using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Domain.Ordering.BuyerInfo;

public sealed class Buyer
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
    
    public IEnumerable<Order>? Orders { get; init; }
}
