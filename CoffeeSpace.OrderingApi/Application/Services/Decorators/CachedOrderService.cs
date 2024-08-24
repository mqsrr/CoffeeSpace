﻿using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using CoffeeSpace.Shared.Attributes;
using CoffeeSpace.Shared.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Services.Decorators;

[Decorator]
internal sealed class CachedOrderService : IOrderService
{
    private readonly ICacheService _cacheService;
    private readonly IOrderService _orderService;
    private readonly IPublisher _publisher;

    public CachedOrderService(ICacheService cacheService, IOrderService orderService, IPublisher publisher)
    {
        _cacheService = cacheService;
        _orderService = orderService;
        _publisher = publisher;
    }

    public Task<IEnumerable<Order>> GetAllByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken)
    {
        return _cacheService.GetAllOrCreateAsync(CacheKeys.Order.GetAll(buyerId), () =>
        {
            var orders = _orderService.GetAllByBuyerIdAsync(buyerId, cancellationToken);
            return orders;
        }, cancellationToken);
    }

    public Task<Order?> GetByIdAsync(Guid id, Guid buyerId, CancellationToken cancellationToken)
    {
        return _cacheService.GetOrCreateAsync(CacheKeys.Order.GetByCustomerId(id, buyerId), () =>
        {
            var order = _orderService.GetByIdAsync(id, buyerId, cancellationToken);
            return order;
        }, cancellationToken);
    }

    public async Task<bool> CreateAsync(Order order, CancellationToken cancellationToken)
    {
        bool isCreated = await _orderService.CreateAsync(order, cancellationToken);
        if (isCreated)
        {
            await _publisher.Publish(new CreateOrderNotification
            {
                Id = order.Id,
                BuyerId = order.BuyerId
            }, cancellationToken).ConfigureAwait(false);
        }
        
        return isCreated;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, Guid buyerId, CancellationToken cancellationToken)
    {
        bool isDeleted = await _orderService.DeleteByIdAsync(id, buyerId, cancellationToken);
        if (isDeleted)
        {
            await _publisher.Publish(new DeleteOrderNotification
            {
                Id = id,
                BuyerId = buyerId
            }, cancellationToken).ConfigureAwait(false);
        }
        
        return isDeleted;

    }
}