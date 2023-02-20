using System.Security.Principal;
using CoffeeSpace.Data.Models.CustomerInfo;

namespace CoffeeSpace.Data.Authentication.Response;

public sealed class JwtResponse
{
    public string Token { get; init; }
    public Customer Customer { get; init; }
    public bool IsSuccess { get; init; }

}