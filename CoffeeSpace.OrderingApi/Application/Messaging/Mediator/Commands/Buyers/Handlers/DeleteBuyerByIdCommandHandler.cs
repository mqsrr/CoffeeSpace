using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using MassTransit;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers.Handlers;

internal sealed class DeleteBuyerByIdCommandHandler : ICommandHandler<DeleteBuyerByIdCommand, bool>
{
    private readonly IBuyerRepository _buyerRepository;
    private readonly ISendEndpointProvider _endpointProvider;

    public DeleteBuyerByIdCommandHandler(IBuyerRepository buyerRepository, ISendEndpointProvider endpointProvider)
    {
        _buyerRepository = buyerRepository;
        _endpointProvider = endpointProvider;
    }

    public async ValueTask<bool> Handle(DeleteBuyerByIdCommand command, CancellationToken cancellationToken)
    {
        var buyer = await _buyerRepository.GetByIdAsync(command.Id, cancellationToken);
        if (buyer is not null)
        {
            var sendEndpoint = await _endpointProvider.GetSendEndpoint(new Uri("queue:delete-buyer"));
            await sendEndpoint.Send<DeleteBuyer>(new
            {
                buyer.Name,
                buyer.Email
            }, cancellationToken);
        }
        
        var deleted = await _buyerRepository.DeleteByIdAsync(command.Id, cancellationToken);
        return deleted;
    }
}