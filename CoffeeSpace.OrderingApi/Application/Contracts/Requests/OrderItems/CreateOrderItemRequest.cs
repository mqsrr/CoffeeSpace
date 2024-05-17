namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.OrderItems;

public sealed class CreateOrderItemRequest
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required int Quantity { get; set; }

    public int Discount { get; set; } = 0;
}