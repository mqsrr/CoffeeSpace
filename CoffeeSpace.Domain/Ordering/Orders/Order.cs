namespace CoffeeSpace.Domain.Ordering.Orders;

public sealed class Order
{
    public required Guid Id { get; init; }
    
    public required OrderStatus Status { get; init; }

    public required Guid BuyerId { get; init; }

    public required Address Address { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}