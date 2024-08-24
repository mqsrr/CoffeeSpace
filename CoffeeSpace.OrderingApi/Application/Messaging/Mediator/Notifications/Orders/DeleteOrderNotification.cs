using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;

public sealed class DeleteOrderNotification : INotification
{
    public required Guid Id { get; init; }

    public required Guid BuyerId { get; init; }
}