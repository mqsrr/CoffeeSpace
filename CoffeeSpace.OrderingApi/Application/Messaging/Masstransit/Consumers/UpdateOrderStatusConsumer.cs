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
        var message = context.Message;
        
        await _orderRepository.UpdateOrderStatusAsync(message.OrderId, context.Message.Status, context.CancellationToken);
        
        await _cacheService.RemoveAsync(CacheKeys.Order.GetByCustomerId(message.OrderId, message.BuyerId), context.CancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.Order.GetAll(message.BuyerId), context.CancellationToken);
    }
}