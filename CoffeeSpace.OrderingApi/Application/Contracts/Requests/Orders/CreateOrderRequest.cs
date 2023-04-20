using CoffeeSpace.Domain.Ordering.CustomerInfo;
using CoffeeSpace.Domain.Ordering.CustomerInfo.CardInfo;
using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;

public sealed class CreateOrderRequest
{
    
    public Guid BuyerId { get; set; }

    public required OrderStatus Status { get; init; }
    
    public required Address Address { get; init; }
    
    public required PaymentInfo PaymentInfo { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}