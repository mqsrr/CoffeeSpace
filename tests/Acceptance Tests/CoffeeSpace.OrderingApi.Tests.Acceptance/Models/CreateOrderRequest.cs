namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

public sealed class CreateOrderRequest
{
    public required OrderStatus Status { get; init; }
    
    public required CreateAddressRequest Address { get; init; }
    
    public required CreatePaymentInfoRequest PaymentInfo { get; init; }
    
    public required IEnumerable<CreateOrderItemRequest> OrderItems { get; init; }
}