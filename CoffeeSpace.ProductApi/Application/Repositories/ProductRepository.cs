using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.ProductApi.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.ProductApi.Application.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly IProductDbContext _productDbContext;

    public ProductRepository(IProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        var count = _productDbContext.Products.CountAsync(cancellationToken);
        return count;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var products = await _productDbContext.Products.ToListAsync(cancellationToken);
        return products;
    }

    public async Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products.FindAsync([id], cancellationToken);
        return product;
    }

    public async Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _productDbContext.Products.AddAsync(product, cancellationToken);
        int result = await _productDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _productDbContext.Products.Update(product);
        int result = await _productDbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? product
            : null;
    }

    public async Task<bool> DeleteProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        int result = await _productDbContext.Products
            .Where(product => product.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        
        return result > 0;
    }
}