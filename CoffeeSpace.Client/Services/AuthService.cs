using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CoffeeSpace.Client.Contracts.Authentication;
using CoffeeSpace.Client.Services.Abstractions;
using CoffeeSpace.Client.WebApiClients;
using Microsoft.IdentityModel.Tokens;


namespace CoffeeSpace.Client.Services;

public sealed class AuthService : IAuthService
{
    private readonly IIdentityWebApi _identityWebApi;

    public AuthService(IIdentityWebApi identityWebApi)
    {
        _identityWebApi = identityWebApi;
    }

    public async Task<bool> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        string token = await _identityWebApi.LoginAsync(request, cancellationToken);
        if (token.IsNullOrEmpty())
        {
            return false;
        }
        
        var tokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(token);
        await SecureStorage.Default.SetAsync("jwt-token", token);
        await SecureStorage.Default.SetAsync("buyer-email", tokenHandler.Claims.First(claim => claim.Type == ClaimTypes.Email).Value);

        return true;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        string token = await _identityWebApi.RegisterAsync(request, cancellationToken);
        if (token.IsNullOrEmpty())
        {
            return false;
        }

        await SecureStorage.Default.SetAsync("jwt-token", token);
        return true;
    }
}