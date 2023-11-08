using CoffeeSpace.Client.Contracts.Authentication;
using CoffeeSpace.Client.Helpers;
using Refit;

namespace CoffeeSpace.Client.WebApiClients;

public interface IIdentityWebApi
{
    [Post(ApiEndpoints.Authentication.Login)]
    Task<string?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    [Post(ApiEndpoints.Authentication.Register)]
    Task<string?> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}