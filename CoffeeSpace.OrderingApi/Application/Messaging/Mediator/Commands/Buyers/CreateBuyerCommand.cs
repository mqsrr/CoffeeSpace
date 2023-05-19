using CoffeeSpace.Domain.Ordering.BuyerInfo;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers;

public sealed class CreateBuyerCommand : ICommand<bool>
{
    public required Buyer Buyer { get; init; }
}