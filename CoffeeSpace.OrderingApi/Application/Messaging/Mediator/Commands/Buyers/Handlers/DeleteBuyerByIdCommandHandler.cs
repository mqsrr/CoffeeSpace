using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers.Handlers;

internal sealed class DeleteBuyerByIdCommandHandler : ICommandHandler<DeleteBuyerByIdCommand, bool>
{
    private readonly IBuyerRepository _buyerRepository;

    public DeleteBuyerByIdCommandHandler(IBuyerRepository buyerRepository)
    {
        _buyerRepository = buyerRepository;
    }

    public async ValueTask<bool> Handle(DeleteBuyerByIdCommand command, CancellationToken cancellationToken)
    {
        var deleted = await _buyerRepository.DeleteByIdAsync(command.Id, cancellationToken);

        return deleted;
    }
}