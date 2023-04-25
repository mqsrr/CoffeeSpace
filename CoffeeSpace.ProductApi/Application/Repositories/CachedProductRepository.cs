using CoffeeSpace.Application.Attributes;
using CoffeeSpace.Application.Services.Abstractions;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Helpers;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;

namespace CoffeeSpace.ProductApi.Application.Repositories;

[Decorator]
internal sealed class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService<Product> _cacheService;

    public CachedProductRepository(IProductRepository productRepository, ICacheService<Product> cacheService)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return _cacheService.GetAllOrCreateAsync(CacheKeys.Products.GetAll, () =>
        {
            var products = _productRepository.GetAllProductsAsync(cancellationToken);
            return products;
        }, cancellationToken);
    }

    public Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _cacheService.GetOrCreateAsync(CacheKeys.Products.GetById(id), () =>
        {
            var product = _productRepository.GetProductByIdAsync(id, cancellationToken);
            return product;
        }, cancellationToken);
    }

    public async Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var created = await _productRepository.CreateProductAsync(product, cancellationToken);
        if (created)
        {
            await _cacheService.RemoveAsync(CacheKeys.Products.GetAll, cancellationToken);
        }

        return created;
    }

    public async Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var updatedProduct = await _productRepository.UpdateProductAsync(product, cancellationToken);
        if (updatedProduct is not null)
        {
            await _cacheService.RemoveAsync(CacheKeys.Products.GetAll, cancellationToken);
            await _cacheService.RemoveAsync(CacheKeys.Products.GetById(updatedProduct.Id), cancellationToken);
        }

        return updatedProduct;
    }

    public async Task<bool> DeleteProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var deleted = await _productRepository.DeleteProductByIdAsync(id, cancellationToken);
        if (deleted)
        {
            await _cacheService.RemoveAsync(CacheKeys.Products.GetAll, cancellationToken);
            await _cacheService.RemoveAsync(CacheKeys.Products.GetById(id), cancellationToken);
        }

        return deleted;
    }
}