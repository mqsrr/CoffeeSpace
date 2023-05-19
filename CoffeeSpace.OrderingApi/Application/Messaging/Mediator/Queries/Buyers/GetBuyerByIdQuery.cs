using CoffeeSpace.Domain.Ordering.BuyerInfo;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Buyers;

public sealed class GetBuyerByIdQuery : IQuery<Buyer?>
{
    public required string Id { get; init; }
}