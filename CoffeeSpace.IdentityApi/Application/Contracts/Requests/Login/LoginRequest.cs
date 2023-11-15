namespace CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;

public sealed class LoginRequest
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}