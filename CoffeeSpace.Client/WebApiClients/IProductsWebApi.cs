using CoffeeSpace.Client.Helpers;
using CoffeeSpace.Client.Models.Products;
using Refit;

namespace CoffeeSpace.Client.WebApiClients;

[Headers("Authorization: Bearer")]
public interface IProductsWebApi
{
    [Get(ApiEndpoints.Products.GetAll)]
    Task<PagedList<Product>> GetAllProductsAsync(CancellationToken cancellationToken);

    [Get(ApiEndpoints.Products.GetById)]
    Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
}