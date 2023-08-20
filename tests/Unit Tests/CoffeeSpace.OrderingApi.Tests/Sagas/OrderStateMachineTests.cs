using AutoFixture;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.Messages.Products.Events;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Sagas;

public sealed class OrderStateMachineTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly ISagaStateMachineTestHarness<OrderStateMachine, OrderStateInstance> _sagaTestHarness;
    private readonly Fixture _fixture;

    public OrderStateMachineTests()
    {
        _fixture = new Fixture();
        var serviceCollection = new ServiceCollection()
            .AddQuartz()
            .AddMassTransitTestHarness(config => config.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>())
            .BuildServiceProvider(true);

        _testHarness = serviceCollection.GetTestHarness();
        _sagaTestHarness = _testHarness.GetSagaStateMachineHarness<OrderStateMachine, OrderStateInstance>();
    }

    [Fact]
    public async Task SubmitOrder_ShouldStartOrderStateMachine()
    {
        // Arrange
        var order = _fixture.Build<Order>()
            .With(order => order.Id, Guid.NewGuid().ToString())
            .With(order => order.BuyerId, Guid.NewGuid().ToString())
            .Create();
        
        // Act
        await _testHarness.Bus.Publish<SubmitOrder>(new
        {
            Order = order
        });

        // Assert
        bool consumedAny = await _sagaTestHarness.Consumed.Any<SubmitOrder>();
        consumedAny.Should().BeTrue();
        
        bool createdAny = await _sagaTestHarness.Created.Any(context => context.CorrelationId == Guid.Parse(order.Id));
        createdAny.Should().BeTrue();
        
        bool requestedStockValidation = await _testHarness.Published.Any<OrderStockValidation>();
        requestedStockValidation.Should().BeTrue();

        var instance = _sagaTestHarness.Created.ContainsInState(Guid.Parse(order.Id), _sagaTestHarness.StateMachine, _sagaTestHarness.StateMachine.Submitted);
        instance.Should().NotBeNull();
    }
    
    public async Task InitializeAsync()
    {
        await _testHarness.Start();
    }

    public async Task DisposeAsync()
    {
        await _testHarness.Stop();
    }
}