using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.PaymentService.Consumers;
using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.PaymentService.Tests.Consumers;

public sealed class OrderPaymentValidationConsumerTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<OrderPaymentValidationConsumer> _consumerTestHarness;
    private readonly Fixture _fixture;
    private readonly IPaymentHistoryRepository _paymentHistoryRepository;

    public OrderPaymentValidationConsumerTests()
    {
        _paymentHistoryRepository = Substitute.For<IPaymentHistoryRepository>();
        var serviceProvider = new ServiceCollection()
            .AddScoped<IPaymentHistoryRepository>(_ => _paymentHistoryRepository)
            .AddMassTransitTestHarness(config => config.AddConsumer<OrderPaymentValidationConsumer>())
            .BuildServiceProvider(true);

        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<OrderPaymentValidationConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldConsumeMessage_AndCheckPayment()
    {
        // Arrange
        _paymentHistoryRepository.CreateAsync(Arg.Any<PaymentHistory>(), Arg.Any<CancellationToken>())
            .Returns(true);
        
        // Act
        var response = await _testHarness.Bus.Request<OrderPaymentValidation, OrderPaymentValidationResult>(new
        {
            Order = _fixture.Create<Order>()
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<OrderPaymentValidation>();
        consumedAny.Should().BeTrue();

        response.Message.Should().NotBeNull();
        await _paymentHistoryRepository.Received().CreateAsync(Arg.Any<PaymentHistory>(), Arg.Any<CancellationToken>());
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