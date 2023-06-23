namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;

public sealed class GetAllOrdersByBuyerIdRequest
{
    public required Guid BuyerId { get; init; }
}