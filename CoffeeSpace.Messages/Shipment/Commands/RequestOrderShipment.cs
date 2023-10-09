using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Shipment.Commands;

public interface RequestOrderShipment
{
    Order Order { get; }
}