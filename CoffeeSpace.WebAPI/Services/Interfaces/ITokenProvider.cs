using System.Security.Claims;
using CoffeeSpace.WebAPI.Dto.Response;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.WebAPI.Services.Interfaces;

public interface ITokenProvider<in TEntity> where TEntity : IdentityUser
{
    Task<JwtTokenResponse> GetTokenAsync(TEntity entity, CancellationToken token = default!);
    Task<JwtTokenResponse> GetTokenAsync(ClaimsPrincipal claimsPrincipal, CancellationToken token = default!);
}