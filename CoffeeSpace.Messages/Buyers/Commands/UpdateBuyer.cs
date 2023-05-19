using CoffeeSpace.Domain.Ordering.BuyerInfo;

namespace CoffeeSpace.Messages.Buyers.Commands;

public sealed record UpdateBuyer
{
    public required Buyer Buyer { get; init; }
}