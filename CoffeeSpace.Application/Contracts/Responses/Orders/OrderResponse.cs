using CoffeeSpace.Domain.Models.Orders;

namespace CoffeeSpace.Application.Contracts.Responses.Orders;

public sealed class OrderResponse
{
    public required string Id { get; init; }
    
    public required OrderStatus Status { get; init; }

    public required IEnumerable<OrderItem> OrderItems { get; init; }
}