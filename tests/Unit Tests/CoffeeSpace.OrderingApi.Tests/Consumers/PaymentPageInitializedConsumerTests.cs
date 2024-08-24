using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Messages.Payment.Commands;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Consumers;

public sealed class PaymentPageInitializedConsumerTests : IAsyncLifetime
{
    private readonly IHubContext<OrderingHub, IOrderingHub> _hubContext;
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<PaymentPageInitializedConsumer> _consumerTestHarness;

    private readonly Fixture _fixture;

    public PaymentPageInitializedConsumerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        _hubContext = _fixture.Create<IHubContext<OrderingHub, IOrderingHub>>();
        
        var serviceCollection = new ServiceCollection()
            .AddTransient<IHubContext<OrderingHub, IOrderingHub>>(_ => _hubContext)
            .AddMassTransitTestHarness(configurator => configurator.AddConsumer<PaymentPageInitializedConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceCollection.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<PaymentPageInitializedConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldSendPaymentPageInitializedMessage()
    {
        // Arrange
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<PaymentPageInitializedConsumer>();
        var orderId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        string? paymentLink = _fixture.Create<string>();

        _hubContext.Clients
            .Group(buyerId.ToString())
            .OrderPaymentPageInitialized(orderId, paymentLink)
            .Returns(Task.CompletedTask);
        
        // Act
        await consumerEndpoint.Send<PaymentPageInitialized>(new
        {
            OrderId = orderId,
            BuyerId = buyerId,
            PaymentApprovalLink = paymentLink,
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<PaymentPageInitialized>();
        consumedAny.Should().BeTrue();

        await _hubContext.Received().Clients.Group(Arg.Any<string>()).OrderPaymentPageInitialized(Arg.Any<Guid>(), Arg.Any<string>());
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