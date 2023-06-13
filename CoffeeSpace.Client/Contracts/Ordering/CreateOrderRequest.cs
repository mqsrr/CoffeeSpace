using CoffeeSpace.Client.Models.Ordering;

namespace CoffeeSpace.Client.Contracts.Ordering;

public sealed class CreateOrderRequest
{
    public required OrderStatus Status { get; init; }
    
    public required CreateAddressRequest CreateAddressRequest { get; init; }
    
    public required CreatePaymentInfoRequest PaymentInfo { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}