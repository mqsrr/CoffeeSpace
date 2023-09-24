namespace CoffeeSpace.IdentityApi.Tests.Acceptance.Models;

public sealed class LoginRequest
{
    public required string Username { get; init; }

    public required string Password { get; init; }
}