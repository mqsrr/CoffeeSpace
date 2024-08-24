using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Buyers;

public sealed class DeleteBuyerNotification : INotification
{
    public required Guid Id { get; init; }
}