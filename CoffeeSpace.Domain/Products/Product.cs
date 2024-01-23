namespace CoffeeSpace.Domain.Products;

public sealed class Product
{
    public required string Id { get; init; }

    public required byte[] Image { get; init; }
    
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }
    
    public  required int Quantity { get; init; }

    public float Total => Quantity * UnitPrice;
}