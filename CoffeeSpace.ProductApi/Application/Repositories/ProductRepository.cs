using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.ProductApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.ProductApi.Application.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly IProductDbContext _productDbContext;

    public ProductRepository(IProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var isNotEmpty = await _productDbContext.Products.AnyAsync(cancellationToken);
        
        return !isNotEmpty
            ? Enumerable.Empty<Product>()
            : _productDbContext.Products;
    }

    public async Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products.FindAsync(new object?[]{ id }, cancellationToken: cancellationToken);

        return product;
    }

    public async Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _productDbContext.Products.AddAsync(product, cancellationToken);
        
        var result = await _productDbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _productDbContext.Products.Update(product);
        
        var result = await _productDbContext.SaveChangesAsync(cancellationToken);
        return result > 0
            ? product
            : null;
    }

    public async Task<bool> DeleteProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products.FindAsync(new object[] {id}, cancellationToken);
        if (product is null)
        {
            return false;
        }

        _productDbContext.Products.Remove(product);
        
        var result = await _productDbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
}