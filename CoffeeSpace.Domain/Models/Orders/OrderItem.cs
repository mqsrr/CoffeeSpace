namespace CoffeeSpace.Domain.Models.Orders;

public sealed class OrderItem
{
    public required string Id { get; init; } = Guid.NewGuid().ToString();

    public required string PictureUrl { get; init; }

    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required int Quantity { get; set; }

    public float Discount { get; set; }

    public float Total
    {
        get => Quantity * UnitPrice;
    }
}