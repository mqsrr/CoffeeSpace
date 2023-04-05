using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Queries.OrderItems;

public sealed class GetOrderItemByIdQuery : IQuery<OrderItem?>
{ 
    public required string Id { get; init; }
}