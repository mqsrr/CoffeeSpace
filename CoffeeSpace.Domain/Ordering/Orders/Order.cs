namespace CoffeeSpace.Domain.Ordering.Orders;

public sealed class Order
{
    public required string Id { get; init; }
    
    public required OrderStatus Status { get; init; }

    public required string BuyerId { get; init; }

    public required Address Address { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}