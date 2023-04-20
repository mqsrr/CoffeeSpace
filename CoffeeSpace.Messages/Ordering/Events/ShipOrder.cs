using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Events;

public interface ShipOrder
{
    Order Order { get; }
}