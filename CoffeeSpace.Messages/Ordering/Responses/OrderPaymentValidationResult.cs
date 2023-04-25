using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Responses;

public interface OrderPaymentValidationResult
{
    Order Order { get; }
    
    bool IsValid { get; }
}