using CoffeeSpace.WebAPI.Dto.Requests;
using CoffeeSpace.WebAPI.Dto.Response;

namespace CoffeeSpace.WebAPI.Services.Interfaces;

public interface IAccountService
{
    Task<JwtTokenResponse> LoginAsync(CustomerLoginModel customer, CancellationToken token = default!);
    Task<JwtTokenResponse> RegisterAsync(CustomerRegisterModel customer, CancellationToken token = default!);
}