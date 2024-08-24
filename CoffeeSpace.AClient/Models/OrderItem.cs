namespace CoffeeSpace.AClient.Models;

public sealed class OrderItem
{
    public required string Id { get; init; }
    
    public required string Title { get; init; }
    
    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required int Quantity { get; set; }
    
    public float Total => Quantity * UnitPrice;
}