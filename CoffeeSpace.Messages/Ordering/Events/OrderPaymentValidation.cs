using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Events;

public interface OrderPaymentValidation
{
    Order Order { get; }
}