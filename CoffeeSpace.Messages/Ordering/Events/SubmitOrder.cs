using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Events;

public interface SubmitOrder
{
    Order Order { get; }
}