using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.PaymentService.Application.Messages.Consumers;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
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
            .AddMassTransitTestHarness(config => config.AddConsumer<OrderPaymentValidationConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<OrderPaymentValidationConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldConsumeMessages_AndCreateNewOrders()
    {
        // Arrange
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<OrderPaymentValidationConsumer>();
        var request = _fixture.Create<RequestOrderPayment>();
        
        // Act
        await consumerEndpoint.Send(request);

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<RequestOrderPayment>();
        consumedAny.Should().BeTrue();

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