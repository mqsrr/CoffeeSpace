using CoffeeSpace.Domain.Products;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Commands;

public sealed class UpdateProductCommand : ICommand<Product?>
{
    public required Product Product { get; init; }
}