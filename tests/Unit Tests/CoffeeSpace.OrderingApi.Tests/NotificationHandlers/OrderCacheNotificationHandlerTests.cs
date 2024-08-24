using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders.Handlers;
using CoffeeSpace.Shared.Services.Abstractions;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.NotificationHandlers;

public sealed class OrderCacheNotificationHandlerTests
{
    private readonly ICacheService _cacheService;
    private readonly OrderCacheNotificationHandler _notificationHandler;

    private readonly Fixture _fixture;
    
    public OrderCacheNotificationHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        _cacheService = _fixture.Create<ICacheService>();

        _notificationHandler = new OrderCacheNotificationHandler(_cacheService);
    }

    [Fact]
    public async Task Handle_CreateOrderNotification_ShouldRemoveAllAvailableCache()
    {
        // Arrange
        var notification = _fixture.Create<CreateOrderNotification>();
        var cancellationToken = CancellationToken.None;
        
        // Act
        await _notificationHandler.Handle(notification, cancellationToken);
        
        // Assert
        await _cacheService.Received().RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId), cancellationToken);
        await _cacheService.Received().RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId), cancellationToken);
        
        await _cacheService.Received().RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId), cancellationToken);
    }
    
    [Fact]
    public async Task Handle_UpdateOrderNotification_ShouldRemoveAllAvailableCache()
    {
        // Arrange
        var notification = _fixture.Create<UpdateOrderNotification>();
        var cancellationToken = CancellationToken.None;
        
        // Act
        await _notificationHandler.Handle(notification, cancellationToken);
        
        // Assert
        await _cacheService.Received().RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId), cancellationToken);
        await _cacheService.Received().RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId), cancellationToken);
            
        await _cacheService.Received().RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId), cancellationToken);
    }
    
    [Fact]
    public async Task Handle_DeleteOrderNotification_ShouldRemoveAllAvailableCache()
    {
        // Arrange
        var notification = _fixture.Create<DeleteOrderNotification>();
        var cancellationToken = CancellationToken.None;
        
        // Act
        await _notificationHandler.Handle(notification, cancellationToken);
        
        // Assert
        await _cacheService.Received().RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId), cancellationToken);
        await _cacheService.Received().RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId), cancellationToken);

        await _cacheService.Received().RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId), cancellationToken);
    }
}