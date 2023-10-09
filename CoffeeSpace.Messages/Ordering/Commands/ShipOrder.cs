using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Commands;

public interface ShipOrder
{
    Order Order { get; }
}