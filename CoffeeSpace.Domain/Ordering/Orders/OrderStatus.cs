namespace CoffeeSpace.Domain.Ordering.Orders;

public enum OrderStatus
{
    Submitted,
    StockConfirmed,
    Paid,
    Shipped,
    Cancelled
}