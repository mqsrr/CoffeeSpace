using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Responses;

public interface OrderPaymentSuccess
{
    string PaypalOrderId { get; }
    
    Order Order { get; }
}