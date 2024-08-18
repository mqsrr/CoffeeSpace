using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Commands;
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
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        var serviceCollection = new ServiceCollection()
            .AddQuartz()
            .AddMassTransitTestHarness(config =>
            {
                config.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>();
            })
            .BuildServiceProvider(true);

        _testHarness = serviceCollection.GetTestHarness();
        _sagaTestHarness = _testHarness.GetSagaStateMachineHarness<OrderStateMachine, OrderStateInstance>();
    }

    [Fact]
    public async Task SubmitOrder_ShouldStartOrderStateMachine()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        
        // Act
        await _testHarness.Bus.Publish<SubmitOrder>(new
        {
            Order = order
        });

        // Assert
        bool consumedAny = await _sagaTestHarness.Consumed.Any<SubmitOrder>();
        consumedAny.Should().BeTrue();
        
        bool createdAny = await _sagaTestHarness.Created.Any(context => context.CorrelationId == order.Id);
        createdAny.Should().BeTrue();

        var instance = _sagaTestHarness.Created.ContainsInState(order.Id, _sagaTestHarness.StateMachine, _sagaTestHarness.StateMachine.Submitted);
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