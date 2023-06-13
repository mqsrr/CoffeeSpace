using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.PaymentInfo;

namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;

public sealed class CreateOrderRequest
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public Guid BuyerId { get; internal set; }

    public required OrderStatus Status { get; init; }
    
    public required CreateAddressRequest Address { get; init; }
    
    public required CreatePaymentInfoRequest PaymentInfo { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}