using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders;

public sealed class DeleteOrderByIdCommand : ICommand<bool>
{
    public required string Id { get; init; }
}