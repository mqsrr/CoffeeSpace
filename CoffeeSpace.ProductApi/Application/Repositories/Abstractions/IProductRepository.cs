using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;

namespace CoffeeSpace.ProductApi.Application.Repositories.Abstractions;

public interface IProductRepository
{
    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);

    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken);
    
    Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken);
    
    Task<bool> DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken);
}