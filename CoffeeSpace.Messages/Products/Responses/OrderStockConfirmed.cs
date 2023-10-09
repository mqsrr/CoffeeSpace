using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Products.Responses;

public interface OrderStockConfirmed
{
    Order Order { get; }
    
    bool IsValid { get; }
}