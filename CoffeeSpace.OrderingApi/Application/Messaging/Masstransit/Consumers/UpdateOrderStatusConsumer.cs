using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;

internal sealed class UpdateOrderStatusConsumer : IConsumer<UpdateOrderStatus>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICacheService<Order> _cacheService;

    public UpdateOrderStatusConsumer(IOrderRepository orderRepository, ICacheService<Order> cacheService)
    {
        _orderRepository = orderRepository;
        _cacheService = cacheService;
    }

    public async Task Consume(ConsumeContext<UpdateOrderStatus> context)
    {
        var order = await _orderRepository.GetByIdAsync(context.Message.OrderId, context.CancellationToken);
        
        await _orderRepository.UpdateOrderStatusAsync(order!, context.Message.Status, context.CancellationToken);
        
        await _cacheService.RemoveAsync(CacheKeys.Order.GetByCustomerId(order!.Id, order.BuyerId), context.CancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(order.BuyerId), context.CancellationToken);
    }
}