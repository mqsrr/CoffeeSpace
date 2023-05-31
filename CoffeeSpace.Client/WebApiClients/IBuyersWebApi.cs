using CoffeeSpace.Client.Contracts.Ordering;
using CoffeeSpace.Client.Helpers;
using Refit;

namespace CoffeeSpace.Client.WebApiClients;

[Headers("Authorization: Bearer")]
public interface IBuyersWebApi
{
    [Get(ApiEndpoints.Buyer.GetById)]
    Task<BuyerResponse> GetBuyerByIdAsync(Guid id, CancellationToken cancellationToken);

    [Get(ApiEndpoints.Buyer.GetByEmail)]
    Task<BuyerResponse> GetBuyerByEmailAsync(string email, CancellationToken cancellationToken);
}