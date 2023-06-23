using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.ProductApi.Application.Services.Abstractions;

namespace CoffeeSpace.ProductApi.Application.Services;

internal sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return _productRepository.GetCountAsync(cancellationToken);
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync(GetAllProductsRequest request, CancellationToken cancellationToken)
    {
        var products = _productRepository.GetAllProductsAsync(request, cancellationToken);
        return products;
    }

    public Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = _productRepository.GetProductByIdAsync(id.ToString(), cancellationToken);
        return product;
    }

    public Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var created = _productRepository.CreateProductAsync(product, cancellationToken);
        return created;
    }

    public Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var updatedProduct = _productRepository.UpdateProductAsync(product, cancellationToken);
        return updatedProduct;
    }

    public Task<bool> DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = _productRepository.DeleteProductByIdAsync(id.ToString(), cancellationToken);
        return deleted;
    }
}