using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using MassTransit;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;

internal sealed class UpdateOrderStatusConsumer : IConsumer<UpdateOrderStatus>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublisher _publisher;

    public UpdateOrderStatusConsumer(IOrderRepository orderRepository, IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _publisher = publisher;
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
        }
    }
}