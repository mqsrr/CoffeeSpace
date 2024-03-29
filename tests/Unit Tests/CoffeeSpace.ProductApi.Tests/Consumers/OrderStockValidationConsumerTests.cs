/*using AutoFixture;
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
    private readonly ITopicProducerProvider _topicProducerProvider;
    private readonly ITopicProducer<OrderStockConfirmed> _topicProducer;
    
    private readonly Fixture _fixture;
    
    public OrderStockValidationConsumerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _topicProducerProvider = Substitute.For<ITopicProducerProvider>();
        _topicProducer = Substitute.For<ITopicProducer<OrderStockConfirmed>>();
        
        var serviceProvider = new ServiceCollection()
            .AddScoped<IProductRepository>(_ => _productRepository)
            .AddScoped<ITopicProducerProvider>(_ => _topicProducerProvider)
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
        var expectedProducts = _fixture.CreateMany<Product>().ToArray();
        
        _productRepository.GetAllProductsAsync(Arg.Any<CancellationToken>())
            .Returns(expectedProducts);
        
        _topicProducerProvider.GetProducer<OrderStockConfirmed>(Arg.Any<Uri>())
            .Returns(_topicProducer);
        
        // Act
        await _testHarness.Bus.Publish<ValidateOrderStock>(new
        {
            Order = _fixture.Create<Order>(),
            ProductTitles = expectedProducts.Select(product => product.Title)
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<ValidateOrderStock>();
        consumedAny.Should().BeTrue();

        consumedAny = await _testHarness.Consumed.Any<OrderStockConfirmed>();
        consumedAny.Should().BeTrue();
        await _productRepository.Received().GetAllProductsAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Consume_ShouldRespondFaultedMessage_WhenProductsStockIsNotValid()
    {
        // Arrange
        var products = _fixture.CreateMany<Product>();
        _productRepository.GetAllProductsAsync(Arg.Any<CancellationToken>())
            .Returns(_fixture.CreateMany<Product>());
        
        // Act
        var response = await _testHarness.Bus.Request<ValidateOrderStock, Fault>(new
        {
            Order = _fixture.Create<Order>(),
            ProductTitles = products.Select(product => product.Title)
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<ValidateOrderStock>();
        consumedAny.Should().BeTrue();

        response.Should().NotBeNull();
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
}*/