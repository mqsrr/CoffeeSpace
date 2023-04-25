using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.CustomerInfo;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers.Handlers;

internal sealed class UpdateBuyerCommandHandler : ICommandHandler<UpdateBuyerCommand, Buyer?>
{
    private readonly IBuyerRepository _buyerRepository;

    public UpdateBuyerCommandHandler(IBuyerRepository buyerRepository)
    {
        _buyerRepository = buyerRepository;
    }

    public async ValueTask<Buyer?> Handle(UpdateBuyerCommand command, CancellationToken cancellationToken)
    {
        var updatedBuyer = await _buyerRepository.UpdateAsync(command.Buyer, cancellationToken);

        return updatedBuyer;
    }
}