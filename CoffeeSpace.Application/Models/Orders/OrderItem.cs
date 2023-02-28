namespace CoffeeSpace.Application.Models.Orders;

public sealed class OrderItem
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public required string PictureUrl { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required decimal UnitPrice { get; init; }
    public required int Quantity { get; set; }

    public decimal Discount { get; set; }
    public decimal Total => Quantity * UnitPrice;
}