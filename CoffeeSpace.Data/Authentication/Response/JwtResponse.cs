using System.Security.Principal;
using CoffeeSpace.Data.Models.CustomerInfo;

namespace CoffeeSpace.Data.Authentication.Response;

public sealed class JwtResponse
{
    public string Token { get; init; } = default!;
    public Customer Customer { get; init; } = default!;
    public bool IsSuccess { get; init; }

}