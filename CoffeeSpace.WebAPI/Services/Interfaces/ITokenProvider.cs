using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.WebAPI.Services.Interfaces;

public interface ITokenProvider<in TEntity> where TEntity : IdentityUser
{
    Task<string> GetTokenAsync(TEntity entity, CancellationToken token = default!);
    Task<string> GetTokenAsync(ClaimsPrincipal claimsPrincipal, CancellationToken token = default!);
}