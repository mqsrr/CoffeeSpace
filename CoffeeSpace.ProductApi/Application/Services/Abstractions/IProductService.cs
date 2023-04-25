using CoffeeSpace.Domain.Products;

namespace CoffeeSpace.ProductApi.Application.Services.Abstractions;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
    
    Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken);
    
    Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken);
    
    Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken);
    
    Task<bool> DeleteProductByIdAsync(string id, CancellationToken cancellationToken);
}