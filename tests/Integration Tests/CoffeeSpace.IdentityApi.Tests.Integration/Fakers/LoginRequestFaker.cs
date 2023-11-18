using AutoBogus;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;

namespace CoffeeSpace.IdentityApi.Tests.Integration.Fakers;

public sealed class LoginRequestFaker : AutoFaker<LoginRequest>
{
    public LoginRequestFaker(string username, string password)
    {
        RuleFor(request => request.Username, username);
        RuleFor(request => request.Password, password);
    }
}