using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Products.Commands;

public interface ValidateOrderStock
{
     Guid Id { get; }
     
     IEnumerable<OrderItem> OrderItems { get; }
}