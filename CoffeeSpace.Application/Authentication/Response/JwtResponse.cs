using CoffeeSpace.Application.Models.CustomerInfo;

namespace CoffeeSpace.Application.Authentication.Response;

public sealed class JwtResponse
{
    public string Token { get; init; } = default!;
    public Customer Customer { get; init; } = default!;
    public bool IsSuccess { get; init; }

}