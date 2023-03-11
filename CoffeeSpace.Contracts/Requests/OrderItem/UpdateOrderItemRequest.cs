namespace CoffeeSpace.Contracts.Requests.OrderItem;

public sealed record UpdateOrderItemRequest(
    string Id,
    string PictureUrl,
    string Title,
    string Description,
    decimal UnitPrice,
    int Quantity,
    decimal Discount);