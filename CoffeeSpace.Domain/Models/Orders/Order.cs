using CoffeeSpace.Domain.Models.CustomerInfo;

namespace CoffeeSpace.Domain.Models.Orders;

public sealed class Order
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    public required string CustomerId { get; init; }

    public required Customer Customer { get; init; }

    public required OrderStatus Status { get; init; }

    public required IEnumerable<OrderItem> OrderItems { get; init; }
}