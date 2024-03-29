using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Products.Commands;

public interface ValidateOrderStock
{
     Order Order { get; }

     IEnumerable<string> ProductTitles { get; }
}