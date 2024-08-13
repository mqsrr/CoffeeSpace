using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Shipment.Commands;

public interface RequestOrderShipment
{
    Guid Id { get; }
    
    Address Address { get; }
}