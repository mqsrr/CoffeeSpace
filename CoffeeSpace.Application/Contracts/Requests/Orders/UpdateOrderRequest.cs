using CoffeeSpace.Domain.Models.Orders;

namespace CoffeeSpace.Application.Contracts.Requests.Orders;

public sealed class UpdateOrderRequest
{
    public OrderStatus Status { get; init; }
}