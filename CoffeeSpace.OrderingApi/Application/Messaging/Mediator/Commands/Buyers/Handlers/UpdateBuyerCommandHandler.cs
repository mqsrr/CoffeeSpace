using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Messages.Buyers.Commands;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using MassTransit;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers.Handlers;

internal sealed class UpdateBuyerCommandHandler : ICommandHandler<UpdateBuyerCommand, Buyer?>
{
    private readonly IBuyerRepository _buyerRepository;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public UpdateBuyerCommandHandler(IBuyerRepository buyerRepository, ISendEndpointProvider sendEndpointProvider)
    {
        _buyerRepository = buyerRepository;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async ValueTask<Buyer?> Handle(UpdateBuyerCommand command, CancellationToken cancellationToken)
    {
        var updatedBuyer = await _buyerRepository.UpdateAsync(command.Buyer, cancellationToken);
        if (updatedBuyer is not null)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:update-buyer"));
            await sendEndpoint.Send<UpdateBuyer>(new
            {
                Buyer = updatedBuyer
            }, cancellationToken);
        }

        return updatedBuyer;
    }
}