using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Contracts.Requests.Auth;
using CoffeeSpace.AClient.Helpers;
using Refit;

namespace CoffeeSpace.AClient.RefitClients;

public interface IIdentityWebApi
{
    [Post(ApiEndpoints.Authentication.Login)]
    Task<IApiResponse<string>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    [Post(ApiEndpoints.Authentication.Register)]
    Task<IApiResponse<string>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}