using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Shipment.Responses;

public interface OrderShipmentResponse
{
    Order Order { get; }
    
    bool ShippingAvailable { get; }
}