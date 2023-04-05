namespace CoffeeSpace.Application.Contracts.Requests.OrderItem;

public sealed class CreateOrderItemRequest
{
    public required string PictureUrl { get; init; }

    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required int Quantity { get; init; }

    public required float Discount { get; init; }
}