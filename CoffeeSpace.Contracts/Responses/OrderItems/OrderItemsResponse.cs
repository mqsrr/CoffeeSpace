namespace CoffeeSpace.Contracts.Responses.OrderItems;

public sealed record OrderItemsResponse(
    List<OrderItemResponse> OrderItemResponses);