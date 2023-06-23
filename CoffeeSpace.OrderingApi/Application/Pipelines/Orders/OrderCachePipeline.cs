using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Orders;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Pipelines.Orders;

internal sealed class OrderCachePipeline :
    IPipelineBehavior<GetOrderByIdQuery, Order?>,
    IPipelineBehavior<GetAllOrdersByBuyerIdQuery, IEnumerable<Order>>
{
    private readonly ICacheService<Order> _cacheService;

    public OrderCachePipeline(ICacheService<Order> cacheService)
    {
        _cacheService = cacheService;
    }

    public async ValueTask<Order?> Handle(GetOrderByIdQuery message, CancellationToken cancellationToken,
        MessageHandlerDelegate<GetOrderByIdQuery, Order?> next)
    {
        var order = await _cacheService.GetOrCreateAsync(CacheKeys.Order.GetByCustomerId(message.Id, message.BuyerId), async () =>
        {
            var order = await next(message, cancellationToken);
            return order;
        }, cancellationToken);

        return order;
    }

    public async ValueTask<IEnumerable<Order>> Handle(GetAllOrdersByBuyerIdQuery message, CancellationToken cancellationToken,
        MessageHandlerDelegate<GetAllOrdersByBuyerIdQuery, IEnumerable<Order>> next)
    {
        var orders = await _cacheService.GetAllOrCreateAsync(CacheKeys.Order.GetAll(message.BuyerId), async () =>
        {
            var orders = await next(message, cancellationToken);
            return orders;
        }, cancellationToken);

        return orders;
    }
}