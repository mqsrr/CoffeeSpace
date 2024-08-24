using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Buyers;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using CoffeeSpace.Shared.Attributes;
using CoffeeSpace.Shared.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Services.Decorators;

[Decorator]
internal sealed class CachedBuyerService : IBuyerService
{
    private readonly ICacheService _cacheService;
    private readonly IBuyerService _buyerService;
    private readonly IPublisher _publisher;

    public CachedBuyerService(ICacheService cacheService, IBuyerService buyerService, IPublisher publisher)
    {
        _cacheService = cacheService;
        _buyerService = buyerService;
        _publisher = publisher;
    }

    public Task<Buyer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _cacheService.GetOrCreateAsync(CacheKeys.Buyers.Get(id), () =>
        {
            var buyer = _buyerService.GetByIdAsync(id, cancellationToken);
            return buyer;
        }, cancellationToken);
    }

    public Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _cacheService.GetOrCreateAsync(CacheKeys.Buyers.GetByEmail(email), () =>
        {
            var buyer = _buyerService.GetByEmailAsync(email, cancellationToken);
            return buyer;
        }, cancellationToken);
    }

    public async Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        bool isCreated = await _buyerService.CreateAsync(buyer, cancellationToken);
        if (isCreated)
        {
            await _publisher.Publish(new CreateBuyerNotification
            {
                Id = buyer.Id,
                Email = buyer.Email
            }, cancellationToken).ConfigureAwait(false);
        }

        return isCreated;
    }

    public async Task<Buyer?> UpdateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        var updatedBuyer = await _buyerService.UpdateAsync(buyer, cancellationToken);
        if (updatedBuyer is not null)
        {
            await _publisher.Publish(new UpdateBuyerNotification
            {
                Id = buyer.Id,
                Email = buyer.Email
            }, cancellationToken).ConfigureAwait(false);
        }

        return updatedBuyer;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        bool isDeleted = await _buyerService.DeleteByIdAsync(id, cancellationToken);
        if (isDeleted)
        {
            await _publisher.Publish(new DeleteBuyerNotification
            {
                Id = id
            }, cancellationToken).ConfigureAwait(false);
        }

        return isDeleted;
    }
}