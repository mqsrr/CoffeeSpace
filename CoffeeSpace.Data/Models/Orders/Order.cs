using CoffeeSpace.Data.Models.CustomerInfo;

namespace CoffeeSpace.Data.Models.Orders;

public sealed class Order
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public OrderStatus Status { get; set; }
    public DateTime AdmittedTime { get; set; }

    public string CustomerId { get; set; } = default!;
    public Customer Customer { get; set; } = default!;
    public IEnumerable<OrderItem> OrderItems { get; set; } = default!;
}