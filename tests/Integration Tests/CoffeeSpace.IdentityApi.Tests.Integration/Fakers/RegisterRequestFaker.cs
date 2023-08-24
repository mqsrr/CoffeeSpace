using AutoBogus;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;

namespace CoffeeSpace.IdentityApi.Tests.Integration.Fakers;

public sealed class RegisterRequestFaker : AutoFaker<RegisterRequest>
{
    public RegisterRequestFaker()
    {
        UseSeed(69);
        
        RuleFor(request => request.Name, faker => faker.Person.FirstName);
        RuleFor(request => request.LastName, faker => faker.Person.LastName);
        RuleFor(request => request.Email, faker => faker.Person.Email);
        RuleFor(request => request.Password, "somePAss123!");
        RuleFor(request => request.UserName, faker => faker.Internet.UserName());
    }
}