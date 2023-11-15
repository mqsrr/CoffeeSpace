using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.ShipmentService.Consumers;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoffeeSpace.ShipmentService.Tests.Consumers;

public sealed class RequestOrderShipmentConsumerTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<RequestOrderShipmentConsumer> _consumerTestHarness;
    private readonly Fixture _fixture;
    
    public RequestOrderShipmentConsumerTests()
    {
        _fixture = new Fixture();
        var serviceProvider = new ServiceCollection()
            .AddMassTransitTestHarness(config => config.AddConsumer<RequestOrderShipmentConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<RequestOrderShipmentConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldConsumeMessage_AndReturnResponse()
    {
        // Arrange
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<RequestOrderShipmentConsumer>();
        
        // Act
        await consumerEndpoint.Send<RequestOrderShipment>(new
        {
            Order = _fixture.Create<Order>()
        });
        
        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<RequestOrderShipment>();
        consumedAny.Should().BeTrue();
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