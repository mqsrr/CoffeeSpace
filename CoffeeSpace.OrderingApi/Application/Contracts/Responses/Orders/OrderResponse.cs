using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Addressses;

namespace CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;

public sealed class OrderResponse
{
    public required string Id { get; init; }
    
    public required OrderStatus Status { get; init; }

    public required AddressResponse Address { get; init; }

    public required IEnumerable<OrderItem> OrderItems { get; init; }
}