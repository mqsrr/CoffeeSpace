using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;

public sealed class UpdateOrderNotification : INotification
{
    public required string Id { get; init; }

    public required string BuyerId { get; init; }
}