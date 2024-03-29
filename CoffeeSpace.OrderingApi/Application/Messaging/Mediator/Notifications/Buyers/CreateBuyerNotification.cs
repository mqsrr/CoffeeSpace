using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Buyers;

public sealed class CreateBuyerNotification : INotification
{
    public required Guid Id { get; init; }

    public required string Email { get; init; }
}