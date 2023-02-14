namespace CoffeeSpace.Data.Models.Orders;

public enum OrderStatus
{
    Submitted,
    AwaitingValidation,
    StockConfirmed,
    Paid,
    Shipped,
    Cancelled
}