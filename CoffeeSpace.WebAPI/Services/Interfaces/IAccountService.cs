using CoffeeSpace.Application.Authentication.Response;
using CoffeeSpace.Contracts.Requests.Customer;

namespace CoffeeSpace.WebAPI.Services.Interfaces;

public interface IAccountService
{
    Task<JwtResponse> LoginAsync(LoginRequest request, CancellationToken token = default!);
    Task<JwtResponse> RegisterAsync(RegisterRequest request, CancellationToken token = default!);
}