using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Buyers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Buyers.Handlers;
using CoffeeSpace.Shared.Services.Abstractions;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.NotificationHandlers;

public sealed class BuyerCacheNotificationHandlersTests
{
    private readonly ICacheService _cacheService;
    private readonly BuyerCacheNotificationHandlers _notificationHandler;

    private readonly Fixture _fixture;

    public BuyerCacheNotificationHandlersTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _cacheService = _fixture.Create<ICacheService>();

        _notificationHandler = new BuyerCacheNotificationHandlers(_cacheService);
    }

    [Fact]
    public async Task Handle_CreateOrderNotification_ShouldRemoveAllAvailableCache()
    {
        // Arrange
        var notification = _fixture.Create<CreateBuyerNotification>();
        var cancellationToken = CancellationToken.None;

        // Act
        await _notificationHandler.Handle(notification, cancellationToken);

        // Assert
        
        await _cacheService.Received().RemoveAsync(CacheKeys.Buyers.Get(notification.Id), cancellationToken);
        await _cacheService.Received().RemoveAsync(CacheKeys.Buyers.GetByEmail(notification.Email), cancellationToken);
    }

    [Fact]
    public async Task Handle_UpdateOrderNotification_ShouldRemoveAllAvailableCache()
    {
        // Arrange
        var notification = _fixture.Create<UpdateBuyerNotification>();
        var cancellationToken = CancellationToken.None;

        // Act
        await _notificationHandler.Handle(notification, cancellationToken);

        // Assert
        await _cacheService.Received().RemoveAsync(CacheKeys.Buyers.Get(notification.Id), cancellationToken);
        await _cacheService.Received().RemoveAsync(CacheKeys.Buyers.GetByEmail(notification.Email), cancellationToken);
    }

    [Fact]
    public async Task Handle_DeleteOrderNotification_ShouldRemoveAllAvailableCache()
    {
        // Arrange
        var notification = _fixture.Create<DeleteBuyerNotification>();
        var cancellationToken = CancellationToken.None;

        // Act
        await _notificationHandler.Handle(notification, cancellationToken);

        // Assert
        await _cacheService.Received().RemoveAsync(CacheKeys.Buyers.Get(notification.Id), cancellationToken);
        await _cacheService.Received().RemoveAsync(CacheKeys.Order.GetAll(notification.Id), cancellationToken);
    }
}