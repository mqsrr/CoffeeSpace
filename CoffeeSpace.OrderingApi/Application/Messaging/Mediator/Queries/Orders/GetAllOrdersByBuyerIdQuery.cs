using CoffeeSpace.Domain.Ordering.Orders;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Orders;

public sealed class GetAllOrdersByBuyerIdQuery : IQuery<IEnumerable<Order>>
{
    public required string BuyerId { get; init; }
}