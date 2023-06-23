using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;

public sealed class CreateOrderNotification : INotification
{
    public required string BuyerId { get; init; }
}