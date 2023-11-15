namespace CoffeeSpace.Client.Models.Products;

public sealed class Product
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required float Discount { get; init; }
    
    public  required int Quantity { get; set; }
}