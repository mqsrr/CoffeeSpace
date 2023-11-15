using CoffeeSpace.Client.Helpers;
using CoffeeSpace.Client.Models.Ordering;
using Refit;

namespace CoffeeSpace.Client.WebApiClients;

[Headers("Authorization: Bearer")]
public interface IBuyersWebApi
{
    [Get(ApiEndpoints.Buyer.GetById)]
    Task<Buyer> GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken);

    [Get(ApiEndpoints.Buyer.GetByEmail)]
    Task<Buyer> GetBuyerByEmailAsync(string email, CancellationToken cancellationToken);
}