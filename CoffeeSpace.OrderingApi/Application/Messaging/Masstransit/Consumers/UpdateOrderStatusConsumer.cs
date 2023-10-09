using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using MassTransit;
using Mediator;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;

public sealed class UpdateOrderStatusConsumer : IConsumer<UpdateOrderStatus>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublisher _publisher;
    private readonly IHubContext<OrderingHub, IOrderingHub> _hubContext;

    public UpdateOrderStatusConsumer(IOrderRepository orderRepository, IPublisher publisher, IHubContext<OrderingHub, IOrderingHub> hubContext)
    {
        _orderRepository = orderRepository;
        _publisher = publisher;
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<UpdateOrderStatus> context)
    {
        var message = context.Message;
        
        bool isStatusUpdated = await _orderRepository.UpdateOrderStatusAsync(message.OrderId, context.Message.Status, context.CancellationToken);
        if (isStatusUpdated)
        {
            await _publisher.Publish(new UpdateOrderNotification
            {
                BuyerId = message.BuyerId,
                Id = message.OrderId
            }, context.CancellationToken);
            
            await _hubContext.Clients.Groups(message.BuyerId, "Web Dashboard").OrderStatusUpdated(message.Status, message.OrderId);
        }
    }
}