using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Buyers;

public sealed class DeleteBuyerByIdCommand : ICommand<bool>
{
    public required string Id { get; init; }
}