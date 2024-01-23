using CoffeeSpace.AClient.Models;
using Mediator;

namespace CoffeeSpace.AClient.Messages.Commands;

public sealed class CreateOrderHistoryCommand : ICommand
{
    public required Order Order { get; init; }
}