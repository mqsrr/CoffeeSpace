using CoffeeSpace.Client.Models.Products;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands;

public sealed class AddProductToCartCommand : ICommand
{
    public required Product Product { get; init; }
}