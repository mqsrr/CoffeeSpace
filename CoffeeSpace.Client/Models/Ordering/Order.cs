namespace CoffeeSpace.Client.Models.Ordering;

public sealed class Order
{
    public required string Id { get; init; }
    
    public required OrderStatus Status { get; set; }

    public required Address Address { get; init; }

    public required IEnumerable<OrderItem> OrderItems { get; init; }
}