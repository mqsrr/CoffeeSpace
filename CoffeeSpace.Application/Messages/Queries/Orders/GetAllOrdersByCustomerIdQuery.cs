using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Queries.Orders;

public sealed class GetAllOrdersByCustomerIdQuery : IQuery<IEnumerable<Order>>
{
    public required string CustomerId { get; init; }
}