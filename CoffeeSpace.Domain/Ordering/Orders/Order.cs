using CoffeeSpace.Domain.Ordering.CustomerInfo;
using CoffeeSpace.Domain.Ordering.CustomerInfo.CardInfo;
using Newtonsoft.Json;

namespace CoffeeSpace.Domain.Ordering.Orders;

public sealed class Order
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    public required OrderStatus Status { get; init; }

    public required string BuyerId { get; init; }

    [JsonIgnore]
    public  Buyer? Buyer { get; init; }
    
    public required string AddressId { get; init; }
    
    public Address? Address { get; init; }

    public required string PaymentInfoId { get; init; }
    
    public PaymentInfo? PaymentInfo { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}