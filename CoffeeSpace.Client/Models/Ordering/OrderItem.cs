namespace CoffeeSpace.Client.Models.Ordering;

public sealed class OrderItem
{
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required int Quantity { get; set; }

    public required float Discount { get; init; }

    public float Total => Quantity * UnitPrice;
}