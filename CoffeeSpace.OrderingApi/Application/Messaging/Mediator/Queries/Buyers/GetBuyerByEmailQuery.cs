using CoffeeSpace.Domain.Ordering.BuyerInfo;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Buyers;

public sealed class GetBuyerByEmailQuery : IQuery<Buyer?>
{
    public required string Email { get; init; }
}