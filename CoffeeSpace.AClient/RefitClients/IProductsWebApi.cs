using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Helpers;
using CoffeeSpace.AClient.Models;
using Refit;

namespace CoffeeSpace.AClient.RefitClients;

[Headers("Bearer")]
public interface IProductsWebApi
{
    [Get(ApiEndpoints.Products.GetAll)]
    Task<IApiResponse<IEnumerable<Product>>> GetAllProductsAsync(CancellationToken cancellationToken);
}