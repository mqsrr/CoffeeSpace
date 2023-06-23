using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Pipelines.Orders;

internal sealed class OrderCacheInvalidationPipeline : 
    IPipelineBehavior<CreateOrderCommand, bool>,
    IPipelineBehavior<UpdateOrderCommand, Order?>,
    IPipelineBehavior<DeleteOrderByIdCommand, bool>
{
    private readonly IPublisher _publisher;

    public OrderCacheInvalidationPipeline(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async ValueTask<bool> Handle(CreateOrderCommand message, CancellationToken cancellationToken,
        MessageHandlerDelegate<CreateOrderCommand, bool> next)
    {
        var created = await next(message, cancellationToken);
        if (created)
        {
            await _publisher.Publish(new CreateOrderNotification
            {
                BuyerId = message.Order.BuyerId
            }, cancellationToken);
        }

        return created;
    }
    
    public async ValueTask<Order?> Handle(UpdateOrderCommand message, CancellationToken cancellationToken,
        MessageHandlerDelegate<UpdateOrderCommand, Order?> next)
    {
        var updatedOrder = await next(message, cancellationToken);
        if (updatedOrder is not null)
        {
            await _publisher.Publish(new UpdateOrderNotification
            {
                Id = message.Order.Id,
                BuyerId = message.Order.BuyerId
            }, cancellationToken);
        }

        return updatedOrder;
    }

    public async ValueTask<bool> Handle(DeleteOrderByIdCommand message, CancellationToken cancellationToken,
        MessageHandlerDelegate<DeleteOrderByIdCommand, bool> next)
    {
        var deleted = await next(message, cancellationToken);
        if (deleted)
        {
            await _publisher.Publish(new DeleteOrderNotification
            {
                Id = message.Id,
                BuyerId = message.BuyerId
            }, cancellationToken);
        }

        return deleted;
    }

}