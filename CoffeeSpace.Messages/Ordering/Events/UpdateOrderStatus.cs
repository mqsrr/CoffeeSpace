using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.Messages.Ordering.Events;

public interface UpdateOrderStatus
{
    public string OrderId { get; }

    public string BuyerId { get; }

    public OrderStatus Status { get; }
}