using CoffeeSpace.Domain.Models.Orders;

namespace CoffeeSpace.Application.Contracts.Requests.Orders;

public sealed class CreateOrderRequest
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    public required string CustomerId { get; init; }

    public required Domain.Models.CustomerInfo.Customer Customer { get; init; }

    public required OrderStatus Status { get; init; }

    public required IEnumerable<Domain.Models.Orders.OrderItem> OrderItems { get; init; }
}