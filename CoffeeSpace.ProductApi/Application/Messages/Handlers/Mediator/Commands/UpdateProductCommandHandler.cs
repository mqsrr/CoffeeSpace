using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Messages.Commands;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Handlers.Mediator.Commands;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Product?>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Product?> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var updatedProduct = await _productRepository.UpdateProductAsync(command.Product, cancellationToken);

        return updatedProduct;
    }
}