using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Commands;

public interface CancelOrder
{
    Order Order { get; }
}