using CoffeeSpace.Domain.Ordering.Orders;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Orders;

public sealed class GetOrderByIdQuery : IQuery<Order?>
{
    public required string Id { get; init; }

    public required string BuyerId { get; init; }
}