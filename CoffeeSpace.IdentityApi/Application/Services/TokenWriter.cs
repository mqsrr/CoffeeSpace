using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using CoffeeSpace.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeSpace.IdentityApi.Application.Services;

internal sealed class TokenWriter : ITokenWriter<ApplicationUser>
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _principalFactory;
    private readonly JwtSettings _jwtSettings;

    public TokenWriter(IOptions<JwtSettings> jwtSettings, IUserClaimsPrincipalFactory<ApplicationUser> principalFactory)
    {
        _principalFactory = principalFactory;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string> WriteTokenAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var claimsPrincipal = await _principalFactory.CreateAsync(user);
        string token = await WriteTokenAsync(claimsPrincipal, cancellationToken);

        return token;
    }

    public Task<string> WriteTokenAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCred = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtSecurityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claimsPrincipal.Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_jwtSettings.Expire),
            signingCred);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
    }
}