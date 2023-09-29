using Mediator;

namespace CoffeeSpace.Client.Messages.Commands;

public sealed class SetNewGlobalBuyerCommand : ICommand<bool>
{
    public required string BuyerEmail { get; init; }
}