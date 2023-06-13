using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.PaymentInfo;

namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;

public sealed class UpdateOrderRequest
{
    public Guid Id { get; internal set; }
    
    public Guid BuyerId { get; internal set; }
    
    public required OrderStatus Status { get; init; }

    public required UpdateAddressRequest Address { get; init; }
    
    public required UpdatePaymentInfoRequest PaymentInfo { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}