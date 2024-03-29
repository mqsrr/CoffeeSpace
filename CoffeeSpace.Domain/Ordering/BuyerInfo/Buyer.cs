using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Domain.Ordering.BuyerInfo;

public sealed class Buyer
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
    
    public IEnumerable<Order>? Orders { get; init; }
}
