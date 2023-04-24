using CoffeeSpace.Application.Services.Abstractions;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Buyers;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Services;

internal sealed class BuyerService : IBuyerService
{
    private readonly ISender _sender;
    private readonly ICacheService<Buyer> _cache;

    public BuyerService(ISender sender, ICacheService<Buyer> cache)
    {
        _sender = sender;
        _cache = cache;
    }

    public Task<Buyer?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return _cache.GetOrCreateAsync(CacheKeys.Buyers.Get(id), async () =>
        {
            var buyer = await _sender.Send(new GetBuyerByIdQuery
            {
                Id = id
            }, cancellationToken);

            return buyer;
        }, cancellationToken);
    }

    public Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _cache.GetOrCreateAsync(CacheKeys.Buyers.GetByEmail(email), async () =>
        {
            var buyer = await _sender.Send(new GetBuyerByEmailQuery
            {
                Email = email
            }, cancellationToken);

            return buyer;
        }, cancellationToken);
    }

    public async Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken = default)
    {
        var created = await _sender.Send(new CreateBuyerCommand
        {
            Buyer = buyer
        }, cancellationToken);

        if (created)
        {
            await _cache.RemoveAsync(CacheKeys.Buyers.Get(buyer.Id), cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Buyers.GetByEmail(buyer.Email), cancellationToken);
        }

        return created;
    }

    public async Task<Buyer?> UpdateAsync(Buyer buyer, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateBuyerCommand
        {
            Buyer = buyer
        }, cancellationToken);

        if (result is not null)
        {
            await _cache.RemoveAsync(CacheKeys.Buyers.Get(buyer.Id), cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Buyers.GetByEmail(buyer.Email), cancellationToken);
        }

        return result;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var deleted = await _sender.Send(new DeleteBuyerByIdCommand
        {
            Id = id
        }, cancellationToken);

        if (deleted)
        {
            await _cache.RemoveAsync(CacheKeys.Buyers.Get(id), cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Order.GetAll(id), cancellationToken);
        }

        return deleted;
    }
}            
        
    
