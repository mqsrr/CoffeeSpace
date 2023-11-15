using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Helpers;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Buyers.Handlers;

internal sealed class BuyerCacheNotificationHandlers : 
    INotificationHandler<CreateBuyerNotification>,
    INotificationHandler<UpdateBuyerNotification>,
    INotificationHandler<DeleteBuyerNotification>
{
    private readonly ICacheService<Buyer> _cacheService;

    public BuyerCacheNotificationHandlers(ICacheService<Buyer> cacheService)
    {
        _cacheService = cacheService;
    }

    public async ValueTask Handle(CreateBuyerNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.Id), cancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.Buyers.GetByEmail(notification.Email), cancellationToken);
    }

    public async ValueTask Handle(UpdateBuyerNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.Id), cancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.Buyers.GetByEmail(notification.Email), cancellationToken);
    }

    public async ValueTask Handle(DeleteBuyerNotification notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(CacheKeys.Buyers.Get(notification.Id), cancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(notification.Id), cancellationToken);
    }
}