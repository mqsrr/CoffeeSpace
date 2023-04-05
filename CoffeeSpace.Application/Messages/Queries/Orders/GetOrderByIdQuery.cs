using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Queries.Orders;

public sealed class GetOrderByIdQuery : IQuery<Order>
{
    public required string Id { get; init; }

    public required string CustomerId { get; init; }

}