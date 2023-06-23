using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Buyers;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Pipelines.Buyers;

internal sealed class BuyerCachePipeline :
    IPipelineBehavior<GetBuyerByIdQuery, Buyer?>,
    IPipelineBehavior<GetBuyerByEmailQuery, Buyer?>
{
    private readonly ICacheService<Buyer> _cacheService;

    public BuyerCachePipeline(ICacheService<Buyer> cacheService)
    {
        _cacheService = cacheService;
    }

    public async ValueTask<Buyer?> Handle(GetBuyerByIdQuery message, CancellationToken cancellationToken,
        MessageHandlerDelegate<GetBuyerByIdQuery, Buyer?> next)
    {
        var buyer = await _cacheService.GetOrCreateAsync(CacheKeys.Buyers.Get(message.Id), async () =>
        {
            var buyer = await next(message, cancellationToken);
            return buyer;
        }, cancellationToken);

        return buyer;
    }

    public async ValueTask<Buyer?> Handle(GetBuyerByEmailQuery message, CancellationToken cancellationToken,
        MessageHandlerDelegate<GetBuyerByEmailQuery, Buyer?> next)
    {
        var buyer = await _cacheService.GetOrCreateAsync(CacheKeys.Buyers.GetByEmail(message.Email), async () =>
        {
            var buyer = await next(message, cancellationToken);
            return buyer;
        }, cancellationToken);

        return buyer;
    }
}