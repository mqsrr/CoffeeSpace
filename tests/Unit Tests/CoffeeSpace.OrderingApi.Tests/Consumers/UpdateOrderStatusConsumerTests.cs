﻿using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Consumers;

public sealed class UpdateOrderStatusConsumerTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<UpdateOrderStatusConsumer> _consumerTestHarness;
    private readonly IPublisher _publisher;
    private readonly IOrderRepository _orderRepository;
    private readonly Fixture _fixture;

    public UpdateOrderStatusConsumerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _orderRepository = _fixture.Create<IOrderRepository>();
        _publisher = _fixture.Create<IPublisher>();
        
        var serviceProvider = new ServiceCollection()
            .AddScoped<IOrderRepository>(_ => _orderRepository)
            .AddScoped<IPublisher>(_ => _publisher)
            .AddMassTransitTestHarness(config => config.AddConsumer<UpdateOrderStatusConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<UpdateOrderStatusConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldConsumeMessages_AndUpdateOrderStatus()
    {
        // Arrange
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<UpdateOrderStatusConsumer>();
        var request = _fixture.Create<UpdateOrderStatus>();

        _orderRepository.UpdateOrderStatusAsync(request.OrderId, request.Status, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        await consumerEndpoint.Send(request);

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<UpdateOrderStatus>();
        consumedAny.Should().BeTrue();

        await _publisher.Received().Publish(Arg.Any<UpdateOrderNotification>(), Arg.Any<CancellationToken>());
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