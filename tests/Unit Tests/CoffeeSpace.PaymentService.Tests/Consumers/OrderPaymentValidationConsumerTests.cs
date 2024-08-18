using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Payment.Commands;
using CoffeeSpace.PaymentService.Application.Messages.Consumers;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.PaymentService.Tests.Consumers;

public sealed class OrderPaymentValidationConsumerTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<OrderPaymentValidationConsumer> _consumerTestHarness;
    private readonly IPaymentService _paymentService;
    private readonly Fixture _fixture;

    public OrderPaymentValidationConsumerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _paymentService = _fixture.Create<IPaymentService>();

        var serviceProvider = new ServiceCollection()
            .AddScoped<IPaymentService>(_ => _paymentService)
            .AddMassTransitTestHarness(config =>
            {
                config.AddConsumer<OrderPaymentValidationConsumer>();
                EndpointConvention.Map<PaymentPageInitialized>(new Uri(EndpointAddresses.Payment.PaymentPageInitialized));

            })
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<OrderPaymentValidationConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldConsumeMessages_AndCreateNewOrders()
    {
        // Arrange
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<OrderPaymentValidationConsumer>();
        var order = _fixture.Create<Order>();
        var paypalOrder = _fixture.Create<PayPalCheckoutSdk.Orders.Order>();

        _paymentService.CreateOrderAsync(order, Arg.Any<CancellationToken>())
            .Returns(paypalOrder);
        
        // Act
        await consumerEndpoint.Send<RequestOrderPayment>(new
        {
            Order = order
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<RequestOrderPayment>();
        consumedAny.Should().BeTrue();
        
        bool isFaultSent = await _testHarness.Sent.Any<Fault<RequestOrderPayment>>();
        isFaultSent.Should().BeFalse();

        await _paymentService.Received().CreateOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Consume_ShouldRespondFaultMessage_WhenOrderWasNotCreated()
    {
        // Arrange
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<OrderPaymentValidationConsumer>();
        var order = _fixture.Create<Order>();

        _paymentService.CreateOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        await consumerEndpoint.Send<RequestOrderPayment>(new
        {
            Order = order
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<RequestOrderPayment>();
        consumedAny.Should().BeTrue();

        bool isSent = await _testHarness.Sent.Any<PaymentPageInitialized>();
        isSent.Should().BeFalse(); 
        
        bool isFaultSent = await _testHarness.Published.Any<Fault<RequestOrderPayment>>();
        isFaultSent.Should().BeTrue();
        
        await _paymentService.Received().CreateOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
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