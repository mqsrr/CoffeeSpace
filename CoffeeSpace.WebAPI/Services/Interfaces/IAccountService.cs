using CoffeeSpace.Data.Authentication.Response;
using CoffeeSpace.WebAPI.Dto.Requests;
using CoffeeSpace.WebAPI.Dto.Response;

namespace CoffeeSpace.WebAPI.Services.Interfaces;

public interface IAccountService
{
    Task<JwtResponse> LoginAsync(CustomerLoginModel customer, CancellationToken token = default!);
    Task<JwtResponse> RegisterAsync(CustomerRegisterModel customer, CancellationToken token = default!);
}