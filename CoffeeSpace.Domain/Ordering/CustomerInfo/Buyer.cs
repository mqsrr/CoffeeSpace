using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Domain.Ordering.CustomerInfo;

public sealed class Buyer
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
    
    public IEnumerable<Order>? Orders { get; init; }
}
