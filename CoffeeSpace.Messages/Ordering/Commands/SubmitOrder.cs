using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Commands;

public interface SubmitOrder
{ 
    Order Order { get; }
}