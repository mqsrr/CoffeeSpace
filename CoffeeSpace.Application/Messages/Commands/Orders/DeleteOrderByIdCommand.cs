using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.Orders;

public class DeleteOrderByIdCommand : ICommand<bool>
{
    public required string Id { get; init; }
}