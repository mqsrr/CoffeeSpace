using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Orders;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using MassTransit;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Services;

internal sealed class OrderService : IOrderService
{
    private readonly ISender _sender;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICacheService<Order> _cache;

    public OrderService(ISender sender, ICacheService<Order> cache, IPublishEndpoint publishEndpoint)
    {
        _sender = sender;
        _cache = cache;
        _publishEndpoint = publishEndpoint;
    }

    public Task<IEnumerable<Order>> GetAllByBuyerIdAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        return _cache.GetAllOrCreateAsync(CacheKeys.Order.GetAll(buyerId), async () =>
        {
            var orders = await _sender.Send(new GetAllOrdersByBuyerIdQuery
            {
                BuyerId = buyerId
            }, cancellationToken);

            return orders;
        }, cancellationToken);
    }

    public Task<Order?> GetByIdAsync(string id, string buyerId, CancellationToken cancellationToken = default)
    {
        return _cache.GetOrCreateAsync(CacheKeys.Order.GetByCustomerId(id, buyerId), async () =>
        {
            var order = await _sender.Send(new GetOrderByIdQuery
            {
                BuyerId = buyerId,
                Id = id
            }, cancellationToken);

            return order;
        }, cancellationToken);
    }

    public async Task<bool> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        var created = await _sender.Send(new CreateOrderCommand
        {
            Order = order
        }, cancellationToken);

        if (created)
        {
            await _cache.RemoveAsync(CacheKeys.Order.GetAll(order.BuyerId), cancellationToken);
            await _publishEndpoint.Publish<SubmitOrder>(new
            {
                Order = order
            }, cancellationToken);
        }

        return created;
    }

    public async Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateOrderCommand
        {
            Order = order,
        }, cancellationToken);

        if (result is not null)
        {
            await _cache.RemoveAsync(CacheKeys.Order.GetAll(order.BuyerId), cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Order.GetByCustomerId(order.Id, order.BuyerId), cancellationToken);
        }

        return result;
    }

    public async Task<bool> DeleteByIdAsync(string id, string buyerId, CancellationToken cancellationToken = default)
    {
        var deleted = await _sender.Send(new DeleteOrderByIdCommand
        {
            Id = id
        }, cancellationToken);

        if (deleted)
        {
            await _cache.RemoveAsync(CacheKeys.Order.GetAll(buyerId), cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Order.GetByCustomerId(id, buyerId), cancellationToken);
        }

        return deleted;
    }
}