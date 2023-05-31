using CoffeeSpace.Client.Contracts.Products;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands;

public sealed class AddProductToCartCommand : ICommand
{
    public required ProductResponse Product { get; init; }
}