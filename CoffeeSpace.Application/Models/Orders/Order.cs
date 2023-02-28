using CoffeeSpace.Application.Models.CustomerInfo;

namespace CoffeeSpace.Application.Models.Orders;

public sealed class Order
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public required OrderStatus Status { get; init; }
    public required DateTime AdmittedTime { get; init; }

    public required string CustomerId { get; init; }
    public required Customer Customer { get; init; }
    public required IEnumerable<OrderItem> OrderItems { get; set; }
}