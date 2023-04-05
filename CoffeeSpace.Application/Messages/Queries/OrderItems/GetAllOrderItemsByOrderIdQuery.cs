using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Queries.OrderItems;

public sealed class GetAllOrderItemsByOrderIdQuery : IQuery<IEnumerable<OrderItem>>
{
    public required string Id { get; init; }
}