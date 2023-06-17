namespace CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;

public sealed class RegisterRequest
{
    public required string Name { get; init; }
    
    public required string LastName { get; init; }

    public required string UserName { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }
}