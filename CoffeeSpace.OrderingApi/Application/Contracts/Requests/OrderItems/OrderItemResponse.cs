namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.OrderItems;

public sealed class OrderItemResponse
{
    public required Guid Id { get; init; }
    
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required int Quantity { get; set; }
    
    public float Total => Quantity * UnitPrice;
}