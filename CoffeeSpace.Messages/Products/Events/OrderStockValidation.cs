using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Domain.Products;

namespace CoffeeSpace.Messages.Products.Events;

public interface OrderStockValidation
{
    Order Order { get; }
    
    IEnumerable<Product> Products { get; }
}