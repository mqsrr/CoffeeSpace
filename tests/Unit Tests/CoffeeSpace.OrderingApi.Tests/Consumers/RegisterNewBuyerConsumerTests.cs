using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Consumers;

public sealed class RegisterNewBuyerConsumerTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<RegisterNewBuyerConsumer> _consumerTestHarness;
    private readonly IBuyerService _buyerService;
    private readonly Fixture _fixture;

    public RegisterNewBuyerConsumerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _buyerService = _fixture.Create<IBuyerService>();
        var serviceProvider = new ServiceCollection()
            .AddScoped<IBuyerService>(_ => _buyerService)
            .AddMassTransitTestHarness(config => config.AddConsumer<RegisterNewBuyerConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<RegisterNewBuyerConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldConsumeMessages_AndRegisterNewBuyer()
    {
        // Arrange
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<RegisterNewBuyerConsumer>();
        var request = _fixture.Create<RegisterNewBuyer>();

        // Act
        await consumerEndpoint.Send(request);

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<RegisterNewBuyer>();
        consumedAny.Should().BeTrue();

        await _buyerService.Received().CreateAsync(Arg.Any<Buyer>(), Arg.Any<CancellationToken>());
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