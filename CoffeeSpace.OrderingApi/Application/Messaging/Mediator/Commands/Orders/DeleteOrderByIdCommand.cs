using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders;

public class DeleteOrderByIdCommand : ICommand<bool>
{
    public required string Id { get; init; }
}