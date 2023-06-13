using CoffeeSpace.Client.Contracts.Authentication;

namespace CoffeeSpace.Client.Services.Abstractions;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    Task<bool> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}