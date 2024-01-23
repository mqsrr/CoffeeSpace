using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Products.Commands;

public interface OrderStockValidation
{
    Order Order { get; }

    IEnumerable<string> ProductTitles { get; }
}