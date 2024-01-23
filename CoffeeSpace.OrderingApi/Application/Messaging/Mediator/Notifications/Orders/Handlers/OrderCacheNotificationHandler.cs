using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.Shared.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders.Handlers;

internal sealed class OrderCacheNotificationHandler : 
    INotificationHandler<CreateOrderNotification>,
    INotificationHandler<UpdateOrderNotification>,
    INotificationHandler<DeleteOrderNotification>
{
    private readonly ICacheService _cacheService;

    public OrderCacheNotificationHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async ValueTask Handle(CreateOrderNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId));
        await _cacheService.RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId));
        
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId));
    }

    public async ValueTask Handle(UpdateOrderNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId));
        await _cacheService.RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId));
            
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId));
    }

    public async ValueTask Handle(DeleteOrderNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId));
        await _cacheService.RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId));
        
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId));
    }
}