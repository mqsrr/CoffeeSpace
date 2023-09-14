using CoffeeSpace.Client.Contracts.Ordering;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands;

public sealed class CreateOrderCommand : ICommand<bool>
{
    public required CreateOrderRequest CreateOrderRequest { get; init; }

    public required Guid BuyerId { get; init; }
}