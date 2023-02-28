namespace CoffeeSpace.Contracts.Responses.OrderItems;

public sealed record OrderItemResponse(
    string Id,
    string PictureUrl,
    string Title,
    string Description,
    decimal UnitPrice, 
    int Quantity,
    decimal Discount);