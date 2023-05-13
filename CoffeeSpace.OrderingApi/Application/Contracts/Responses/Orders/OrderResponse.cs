using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;

public sealed class OrderResponse
{
    public required string Id { get; init; }
    
    public required OrderStatus Status { get; init; }

    public required Address Address { get; init; }

    public required IEnumerable<OrderItem> OrderItems { get; init; }
}