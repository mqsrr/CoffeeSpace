namespace CoffeeSpace.Contracts.Requests.Customer;

public sealed record LoginRequest(
    string Username,
    string Email,
    string Password);