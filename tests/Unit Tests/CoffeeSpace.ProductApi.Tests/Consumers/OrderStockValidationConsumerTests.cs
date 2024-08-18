using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
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
                
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        _productRepository = _fixture.Create<IProductRepository>();
        var serviceProvider = new ServiceCollection()
            .AddScoped<IProductRepository>(_ => _productRepository)
            .AddMassTransitTestHarness(config => config.AddConsumer<OrderStockValidationConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<OrderStockValidationConsumer>();

    }

    [Fact]
    public async Task Consume_ShouldConsumeMessage_AndCheckProductsStockAvailability()
    {
        // Arrange
        var expectedProducts = _fixture.CreateMany<Product>().ToArray();
        var orderId = Guid.NewGuid();

        _productRepository.GetAllProductsAsync(Arg.Any<CancellationToken>())
            .Returns(expectedProducts);
        
        // Act
        await _testHarness.Bus.Publish<ValidateOrderStock>(new
        {
            Id = orderId,
            OrderItems = expectedProducts,
        });


        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<ValidateOrderStock>();
        consumedAny.Should().BeTrue();
        
        bool isSent = await _testHarness.Published.Any<OrderStockConfirmed>();
        isSent.Should().BeTrue();
        
        await _productRepository.Received().GetAllProductsAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Consume_ShouldRespondWithFaultedMessage_WhenProductsStockIsNotValid()
    {
        // Arrange
        _productRepository.GetAllProductsAsync(Arg.Any<CancellationToken>())
            .Returns(_fixture.CreateMany<Product>(5));
        
        // Act
        await _testHarness.Bus.Publish<ValidateOrderStock>(new
        {
            Id = Guid.NewGuid(),
            OrderItems = _fixture.CreateMany<OrderItem>(5),
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<ValidateOrderStock>();
        consumedAny.Should().BeTrue();
        
        bool isFaultSent = await _testHarness.Published.Any<Fault<ValidateOrderStock>>();
        isFaultSent.Should().BeTrue();

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