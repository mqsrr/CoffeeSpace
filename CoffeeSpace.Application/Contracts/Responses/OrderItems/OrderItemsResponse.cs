namespace CoffeeSpace.Application.Contracts.Responses.OrderItems;

public sealed class OrderItemsResponse
{
    public required List<OrderItemResponse> OrderItemResponses { get; init; }
}