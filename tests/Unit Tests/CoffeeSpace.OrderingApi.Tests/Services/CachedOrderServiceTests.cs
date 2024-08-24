using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Decorators;
using CoffeeSpace.Shared.Services.Abstractions;
using FluentAssertions;
using Mediator;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Services;

public sealed class CachedOrderServiceTests
{
    private readonly ICacheService _cacheService;
    private readonly IOrderService _orderService;
    private readonly IPublisher _publisher;

    private readonly Fixture _fixture;
    private readonly CachedOrderService _cachedOrderService;

    public CachedOrderServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        _cacheService = _fixture.Create<ICacheService>();
        _orderService = _fixture.Create<IOrderService>();
        _publisher = _fixture.Create<IPublisher>();

        _cachedOrderService = new CachedOrderService(_cacheService, _orderService, _publisher);
    }

    [Fact]
    public async Task GetAllByOrderIdAsync_ShouldReturnAllCachedOrders_WhenCachedOrdersExist()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orders = _fixture.CreateMany<Order>(5).ToList();
        var cancellationToken = CancellationToken.None;

        _cacheService.GetAllOrCreateAsync(CacheKeys.Order.GetAll(orderId), Arg.Any<Func<Task<IEnumerable<Order>>>>()!, cancellationToken)
            .Returns(orders);
        
        // Act
        var ordersResult = await _cachedOrderService.GetAllByBuyerIdAsync(orderId, cancellationToken);
        ordersResult = ordersResult.ToList();

        // Assert
        ordersResult.Should().NotBeEmpty();
        ordersResult.Should().BeEquivalentTo(orders);
        
        await _orderService.DidNotReceive().GetAllByBuyerIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetAllByOrderIdAsync_ShouldReturnAllOrders_AndCreateCache_WhenOrdersExist()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orders = _fixture.CreateMany<Order>(5).ToList();
        var cancellationToken = CancellationToken.None;

        _cacheService.GetAllOrCreateAsync(CacheKeys.Order.GetAll(orderId), Arg.Any<Func<Task<IEnumerable<Order>>>>()!, cancellationToken)
            .Returns(info => info.Arg<Func<Task<IEnumerable<Order>>>>().Invoke());

        _orderService.GetAllByBuyerIdAsync(orderId, cancellationToken)
            .Returns(orders);
        
        // Act
        var ordersResult = await _cachedOrderService.GetAllByBuyerIdAsync(orderId, cancellationToken);
        ordersResult = ordersResult.ToList();
        
        // Assert
        ordersResult.Should().NotBeEmpty();
        ordersResult.Should().BeEquivalentTo(orders);
        
        await _orderService.Received().GetAllByBuyerIdAsync(orderId, cancellationToken);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnCachedOrder_WhenCachedOrderExists()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        var cancellationToken = CancellationToken.None;

        _cacheService.GetOrCreateAsync(CacheKeys.Order.GetByCustomerId(order.Id, order.BuyerId), Arg.Any<Func<Task<Order>>>()!, cancellationToken)
            .Returns(order);
        
        // Act
        var orderResult = await _cachedOrderService.GetByIdAsync(order.Id, order.BuyerId, cancellationToken);
        
        // Assert
        orderResult.Should().NotBeNull();
        orderResult.Should().BeEquivalentTo(order);

        await _orderService.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrder_AndCreateCache_WhenOrderExists()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        var cancellationToken = CancellationToken.None;

        _cacheService.GetOrCreateAsync(CacheKeys.Order.GetByCustomerId(order.Id, order.BuyerId), Arg.Any<Func<Task<Order>>>()!, cancellationToken)!
            .Returns(info => info.Arg<Func<Task<Order>>>().Invoke());

        _orderService.GetByIdAsync(order.Id, order.BuyerId, cancellationToken)
            .Returns(order);
        
        // Act
        var orderResult = await _cachedOrderService.GetByIdAsync(order.Id, order.BuyerId, cancellationToken);
        
        // Assert
        orderResult.Should().NotBeNull();
        orderResult.Should().BeEquivalentTo(order);

        await _orderService.Received().GetByIdAsync(order.Id, order.BuyerId, cancellationToken);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_AndInvalidateCache_WhenOrderWasCreated()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        var cancellationToken = CancellationToken.None;

        _orderService.CreateAsync(order, cancellationToken)
            .Returns(true);

        _publisher.Publish(Arg.Any<CreateOrderNotification>(), cancellationToken)
            .Returns(ValueTask.CompletedTask);
        
        // Act
        bool isCreated = await _cachedOrderService.CreateAsync(order, cancellationToken);
        
        // Assert
        isCreated.Should().BeTrue();

        await _orderService.Received().CreateAsync(order, cancellationToken);
        await _publisher.Received().Publish(Arg.Any<CreateOrderNotification>(), cancellationToken);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_AndKeepCache_WhenOrderWasNotCreated()
    {
        // Arrange
        var order = _fixture.Create<Order>();
        var cancellationToken = CancellationToken.None;

        _orderService.CreateAsync(order, cancellationToken)
            .Returns(false);
        
        // Act
        bool isCreated = await _cachedOrderService.CreateAsync(order, cancellationToken);
        
        // Assert
        isCreated.Should().BeFalse();

        await _orderService.Received().CreateAsync(order, cancellationToken);
        await _publisher.DidNotReceive().Publish(Arg.Any<INotification>(), cancellationToken);
    }    
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_AndInvalidateCache_WhenOrderWasDeleted()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _orderService.DeleteByIdAsync(orderId, buyerId, cancellationToken)
            .Returns(true);

        _publisher.Publish(Arg.Any<DeleteOrderNotification>(), cancellationToken)
            .Returns(ValueTask.CompletedTask);
        
        // Act
        bool isDeleted = await _cachedOrderService.DeleteByIdAsync(orderId, buyerId, cancellationToken);
        
        // Assert
        isDeleted.Should().BeTrue();

        await _orderService.Received().DeleteByIdAsync(orderId, buyerId, cancellationToken);
        await _publisher.Received().Publish(Arg.Any<DeleteOrderNotification>(), cancellationToken);
    }

    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_AndKeepCache_WhenOrderWasNotDeleted()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _orderService.DeleteByIdAsync(orderId, buyerId, cancellationToken)
            .Returns(false);
        
        // Act
        bool isDeleted = await _cachedOrderService.DeleteByIdAsync(orderId, buyerId, cancellationToken);
        
        // Assert
        isDeleted.Should().BeFalse();

        await _orderService.Received().DeleteByIdAsync(orderId, buyerId, cancellationToken);
        await _publisher.DidNotReceive().Publish(Arg.Any<INotification>(), cancellationToken);
    }

}