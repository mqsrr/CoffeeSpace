using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Responses;

public interface OrderPaid
{
    string PaypalOrderId { get; }
    
    Order Order { get; }
}