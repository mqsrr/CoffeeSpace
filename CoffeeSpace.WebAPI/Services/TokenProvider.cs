using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoffeeSpace.WebAPI.Dto.Response;
using CoffeeSpace.WebAPI.Options;
using CoffeeSpace.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeSpace.WebAPI.Services;

public sealed class TokenProvider<TEntity> : ITokenProvider<TEntity>
    where TEntity : IdentityUser
{
    private readonly IUserClaimsPrincipalFactory<TEntity> _claimsPrincipalFactory;
    private readonly JwtOption _jwtSettings;
    
    public TokenProvider(IUserClaimsPrincipalFactory<TEntity> claimsPrincipalFactory, IOptionsSnapshot<JwtOption> jwtSettings)
    {
        _claimsPrincipalFactory = claimsPrincipalFactory;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<JwtTokenResponse> GetTokenAsync(TEntity entity, CancellationToken token = default)
    {
        ClaimsPrincipal claimsPrincipal = await _claimsPrincipalFactory.CreateAsync(entity);
        
        JwtTokenResponse jwtTokenResponse = await GetTokenAsync(claimsPrincipal, token);

        return jwtTokenResponse;
    }

    public Task<JwtTokenResponse> GetTokenAsync(ClaimsPrincipal claimsPrincipal, CancellationToken token = default)
    {
        SymmetricSecurityKey symmetricKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        SigningCredentials signingCred =
            new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(_jwtSettings.Issuer,
            _jwtSettings.Audience,
            claimsPrincipal.Claims, 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiredTime), 
            signingCred);

        JwtTokenResponse jwtTokenResponse =
            new JwtTokenResponse(token: new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));

        return Task.FromResult(jwtTokenResponse);
    }
}