using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Helpers;

namespace CoffeeSpace.ProductApi.Application.Services.Abstractions;

public interface IProductService
{
    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
    
    Task<PagedList<Product>> GetAllProductsAsync(int page, int pageSize, CancellationToken cancellationToken);
    
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken);
    
    Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken);
    
    Task<bool> DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken);
}