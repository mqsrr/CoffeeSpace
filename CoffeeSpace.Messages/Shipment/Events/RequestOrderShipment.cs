using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Shipment.Events;

public interface RequestOrderShipment
{
    Order Order { get; }
}