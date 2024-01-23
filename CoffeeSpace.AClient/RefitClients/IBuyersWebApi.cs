using System;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Helpers;
using CoffeeSpace.AClient.Models;
using Refit;

namespace CoffeeSpace.AClient.RefitClients;

[Headers("Authorization: Bearer")]
public interface IBuyersWebApi
{
    [Get(ApiEndpoints.Buyer.GetById)]
    Task<Buyer> GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken);

    [Get(ApiEndpoints.Buyer.GetByEmail)]
    Task<Buyer> GetBuyerByEmailAsync(string email, CancellationToken cancellationToken);
}