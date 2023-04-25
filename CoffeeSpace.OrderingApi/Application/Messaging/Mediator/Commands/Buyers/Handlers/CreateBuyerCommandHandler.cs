using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers.Handlers;

internal sealed class CreateBuyerCommandHandler : ICommandHandler<CreateBuyerCommand, bool>
{
    private readonly IBuyerRepository _buyerRepository;

    public CreateBuyerCommandHandler(IBuyerRepository buyerRepository)
    {
        _buyerRepository = buyerRepository;
    }

    public async ValueTask<bool> Handle(CreateBuyerCommand command, CancellationToken cancellationToken)
    {
        var created = await _buyerRepository.CreateAsync(command.Buyer, cancellationToken);

        return created;
    }
}