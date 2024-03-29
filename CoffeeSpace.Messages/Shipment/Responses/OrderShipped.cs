using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Shipment.Responses;

public interface OrderShipped
{
    Order Order { get; }
    
    bool ShippingAvailable { get; }
}