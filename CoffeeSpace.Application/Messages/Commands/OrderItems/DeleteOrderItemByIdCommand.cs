using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.OrderItems;

public sealed class DeleteOrderItemByIdCommand : ICommand<bool>
{
    public required string Id { get; init; }
}