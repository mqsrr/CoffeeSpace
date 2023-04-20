using CoffeeSpace.Domain.Products;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Commands;

public sealed class CreateProductCommand : ICommand<bool>
{
    public required Product Product { get; init; }
}