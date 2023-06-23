using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;

namespace CoffeeSpace.ProductApi.Application.Services.Abstractions;

public interface IProductService
{
    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetAllProductsAsync(GetAllProductsRequest request, CancellationToken cancellationToken);
    
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<bool> CreateProductAsync(Product product, CancellationToken cancellationToken);
    
    Task<Product?> UpdateProductAsync(Product product, CancellationToken cancellationToken);
    
    Task<bool> DeleteProductByIdAsync(Guid id, CancellationToken cancellationToken);
}