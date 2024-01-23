using CoffeeSpace.AClient.Models;
using Mediator;

namespace CoffeeSpace.AClient.Messages.Commands;

public sealed class AddToCartCommand : ICommand
{
    public required Product Product { get; init; }
}