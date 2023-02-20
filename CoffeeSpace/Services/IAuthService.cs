using CoffeeSpace.Data.Authentication.Response;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Dto;

namespace CoffeeSpace.Services;

public interface IAuthService
{
    Task<JwtResponse> LoginAsync(CustomerLoginModel loginModel, CancellationToken token = default!);
    Task<JwtResponse> RegisterAsync(CustomerRegisterModel registerModel, CancellationToken token = default!);
}