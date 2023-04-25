using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Events;

public interface CancelOrder
{
    Order Order { get; }
}