using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Messages.Products.Events;
using CoffeeSpace.ProductApi.Application.Messages.Consumers;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.ProductApi.Tests.Consumers;

public sealed class OrderStockValidationConsumerTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<OrderStockValidationConsumer> _consumerTestHarness;
    private readonly IProductRepository _productRepository;
    private readonly Fixture _fixture;
    
    public OrderStockValidationConsumerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        var serviceProvider = new ServiceCollection()
            .AddScoped<IProductRepository>(_ => _productRepository)
            .AddMassTransitTestHarness(config => config.AddConsumer<OrderStockValidationConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<OrderStockValidationConsumer>();
        
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task Consume_ShouldConsumeMessage_AndCheckProductsStockAvailability()
    {
        // Arrange
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<OrderStockValidationConsumer>();
        var request = _fixture.Create<OrderStockValidation>();
        
        // Act
        await consumerEndpoint.Send(request);

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<OrderStockValidation>();
        consumedAny.Should().BeTrue();

        await _productRepository.Received().GetAllProductsAsync(Arg.Any<CancellationToken>());
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