using AutoBogus;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;

namespace CoffeeSpace.IdentityApi.Tests.Integration.Fakers;

public sealed class FaultRegisterRequestFaker : AutoFaker<RegisterRequest>
{
    public FaultRegisterRequestFaker()
    {
        RuleFor(request => request.Name, faker => faker.Person.FirstName);
        RuleFor(request => request.LastName, faker => faker.Person.LastName);
        RuleFor(request => request.Email, faker => faker.Person.FirstName);
        RuleFor(request => request.Password, "somepasss");
        RuleFor(request => request.UserName, faker => faker.Internet.UserName());
    }
}