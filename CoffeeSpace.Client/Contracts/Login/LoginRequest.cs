namespace CoffeeSpace.Client.Contracts.Login;

public sealed class LoginRequest
{
    public string Username { get; init; }

    public string Password { get; init; }
}