using AutoBogus;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;

namespace CoffeeSpace.IdentityApi.Tests.Integration.Fakers;

public sealed class LoginRequestFaker : AutoFaker<LoginRequest>
{
    public LoginRequestFaker(string email, string password)
    {
        RuleFor(request => request.Email, email);
        RuleFor(request => request.Password, password);
    }
}