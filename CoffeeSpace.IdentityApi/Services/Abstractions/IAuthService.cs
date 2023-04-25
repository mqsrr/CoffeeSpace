namespace CoffeeSpace.IdentityApi.Services.Abstractions;

public interface IAuthService<in TUser>
{
    Task<string?> RegisterAsync(TUser user, CancellationToken cancellationToken);
    
    Task<string?> LoginAsync(TUser user, CancellationToken cancellationToken);
    
    Task<string?> LoginAsync(string username, string password, CancellationToken cancellationToken);
}