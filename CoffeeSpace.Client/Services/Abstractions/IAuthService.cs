using CoffeeSpace.Client.Contracts.Login;
using CoffeeSpace.Client.Contracts.Register;

namespace CoffeeSpace.Client.Services.Abstractions;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    Task<bool> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
}