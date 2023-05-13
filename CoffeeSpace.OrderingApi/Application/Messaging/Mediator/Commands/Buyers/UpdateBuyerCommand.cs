using CoffeeSpace.Domain.Ordering.BuyerInfo;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers;

public sealed class UpdateBuyerCommand : ICommand<Buyer?>
{
    public required Buyer Buyer { get; init; }
}