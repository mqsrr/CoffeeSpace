using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Products.Responses;

public interface OrderStockValidationResult
{
    Order Order { get; }
    
    bool IsValid { get; }
}