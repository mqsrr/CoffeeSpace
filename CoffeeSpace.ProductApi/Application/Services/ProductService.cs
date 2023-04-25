using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Messages.Commands;
using CoffeeSpace.ProductApi.Application.Messages.Queries;
using CoffeeSpace.ProductApi.Application.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Services;

internal sealed class ProductService : IProductService
{
    private readonly ISender _sender;

    public ProductService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var products = await _sender.Send(new GetAllProductsQuery(), cancellationToken);

        return products;
    }

    public async Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = await _sender.Send(new GetProductByIdQuery
        {
            Id = id
        }, cancellationToken);

        return product;
    }

    public async Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var created = await _sender.Send(new CreateProductCommand
        {
            Product = product
        }, cancellationToken);

        return created;
    }

    public async Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var updateProduct = await _sender.Send(new UpdateProductCommand
        {
            Product = product
        }, cancellationToken);

        return updateProduct;
    }

    public async Task<bool> DeleteProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var deleted = await _sender.Send(new DeleteProductByIdCommand
        {
            Id = id
        }, cancellationToken);

        return deleted;
    }
}