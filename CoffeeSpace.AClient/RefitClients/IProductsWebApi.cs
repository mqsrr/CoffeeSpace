using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Helpers;
using CoffeeSpace.AClient.Models;
using Refit;

namespace CoffeeSpace.AClient.RefitClients;

[Headers("Authorization: Bearer")]
public interface IProductsWebApi
{
    [Get(ApiEndpoints.Products.GetAll)]
    Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);

    [Get(ApiEndpoints.Products.GetById)]
    Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
}