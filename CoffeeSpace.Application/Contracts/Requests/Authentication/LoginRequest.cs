namespace CoffeeSpace.Application.Contracts.Requests.Authentication;

public sealed class LoginRequest
{
    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }
}