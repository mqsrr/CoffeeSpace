using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Commands;

public interface UpdateOrderStatus
{
    string OrderId { get; }

    string BuyerId { get; }

    OrderStatus Status { get; }
}