namespace CoffeeSpace.Contracts.Requests.OrderItem;

public sealed record CreateOrderItemRequest(
    string PictureUrl,
    string Title,
    string Description,
    decimal UnitPrice,
    int Quantity,
    decimal Discount);