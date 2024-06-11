using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Helpers;
using CoffeeSpace.AClient.Models;
using Refit;

namespace CoffeeSpace.AClient.RefitClients;

[Headers("Bearer")]
public interface IBuyersWebApi
{
    [Get(ApiEndpoints.Buyer.GetByEmail)]
    Task<IApiResponse<Buyer>> GetBuyerByEmailAsync(string email, CancellationToken cancellationToken);
}