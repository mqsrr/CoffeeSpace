using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Helpers;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders.Handlers;

internal sealed class OrderCacheNotificationHandler : 
    INotificationHandler<CreateOrderNotification>,
    INotificationHandler<UpdateOrderNotification>,
    INotificationHandler<DeleteOrderNotification>
{
    private readonly ICacheService<Order> _cacheService;

    public OrderCacheNotificationHandler(ICacheService<Order> cacheService)
    {
        _cacheService = cacheService;
    }

    public async ValueTask Handle(CreateOrderNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId), cancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId), cancellationToken);
        
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId), cancellationToken);
    }

    public async ValueTask Handle(UpdateOrderNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId), cancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId), cancellationToken);
            
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId), cancellationToken);
    }

    public async ValueTask Handle(DeleteOrderNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(notification.BuyerId), cancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.Order.GetByCustomerId(notification.Id, notification.BuyerId), cancellationToken);
        
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.BuyerId), cancellationToken);
    }
}