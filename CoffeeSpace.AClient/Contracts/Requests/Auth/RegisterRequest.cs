namespace CoffeeSpace.AClient.Contracts.Requests.Auth;

public sealed class RegisterRequest
{
    public required string Username { get; init; }
    
    public required string Name { get; init; }
    
    public required string LastName { get; init; }
    
    public required string Password { get; init; }
    
    public required string Email { get; init; }
}