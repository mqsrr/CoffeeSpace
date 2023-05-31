using CoffeeSpace.Client.Contracts.Products;
using CoffeeSpace.Client.Helpers;
using Refit;

namespace CoffeeSpace.Client.WebApiClients;

[Headers("Authorization: Bearer")]
public interface IProductsWebApi
{
    [Get(ApiEndpoints.Products.GetAll)]
    Task<IEnumerable<ProductResponse>> GetAllProductsAsync(CancellationToken cancellationToken);

    [Get(ApiEndpoints.Products.GetById)]
    Task<ProductResponse> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
}