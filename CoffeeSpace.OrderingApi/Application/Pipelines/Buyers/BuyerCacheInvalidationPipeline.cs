using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Buyers;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Pipelines.Buyers;

internal sealed class BuyerCacheInvalidationPipeline : 
    IPipelineBehavior<CreateBuyerCommand, bool>,
    IPipelineBehavior<UpdateBuyerCommand, Buyer?>,
    IPipelineBehavior<DeleteBuyerByIdCommand, bool>
{
    private readonly IPublisher _publisher;

    public BuyerCacheInvalidationPipeline(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async ValueTask<bool> Handle(CreateBuyerCommand message, CancellationToken cancellationToken,
        MessageHandlerDelegate<CreateBuyerCommand, bool> next)
    {
        var created = await next(message, cancellationToken);
        if (created)
        {
            await _publisher.Publish(new CreateBuyerNotification
            {
                Id = message.Buyer.Id,
                Email = message.Buyer.Email
            }, cancellationToken);
        }

        return created;
    }
    
    public async ValueTask<Buyer?> Handle(UpdateBuyerCommand message, CancellationToken cancellationToken,
        MessageHandlerDelegate<UpdateBuyerCommand, Buyer?> next)
    {
        var updatedBuyer = await next(message, cancellationToken);
        if (updatedBuyer is not null)
        {
            await _publisher.Publish(new UpdateBuyerNotification
            {
                Id = message.Buyer.Id,
                Email = message.Buyer.Email
            }, cancellationToken);
        }

        return updatedBuyer;
    }
    
    public async ValueTask<bool> Handle(DeleteBuyerByIdCommand message, CancellationToken cancellationToken,
        MessageHandlerDelegate<DeleteBuyerByIdCommand, bool> next)
    {
        var deleted = await next(message, cancellationToken);
        if (deleted)
        {
            await _publisher.Publish(new DeleteBuyerNotification
            {
                Id = message.Id
            }, cancellationToken);
        }

        return deleted;
    }
}