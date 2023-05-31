namespace CoffeeSpace.Client.Contracts.Products;

public sealed class ProductResponse
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required float Discount { get; init; }

    public required int Quantity { get; set; }

    public float Total => Quantity * UnitPrice;
}