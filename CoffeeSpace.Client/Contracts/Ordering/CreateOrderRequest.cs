using CoffeeSpace.Client.Models.Ordering;

namespace CoffeeSpace.Client.Contracts.Ordering;

public sealed class CreateOrderRequest
{
    public required int Status { get; init; }
    
    public required CreateAddressRequest Address { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}