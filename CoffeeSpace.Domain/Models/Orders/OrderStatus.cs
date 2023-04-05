namespace CoffeeSpace.Domain.Models.Orders;

public enum OrderStatus
{
    Submitted,
    AwaitingValidation,
    StockConfirmed,
    Paid,
    Shipped,
    Cancelled
}