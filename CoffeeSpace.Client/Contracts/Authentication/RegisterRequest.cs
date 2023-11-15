namespace CoffeeSpace.Client.Contracts.Authentication;

public sealed class RegisterRequest
{
    public string Name { get; init; }

    public string LastName { get; init; }

    public string Username { get; init; }

    public string Email { get; init; }

    public string Password { get; init; }
}