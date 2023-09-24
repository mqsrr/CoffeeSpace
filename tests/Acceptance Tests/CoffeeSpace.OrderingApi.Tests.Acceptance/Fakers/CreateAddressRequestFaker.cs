using AutoBogus;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Fakers;

internal sealed class CreateAddressRequestFaker : AutoFaker<CreateAddressRequest>
{
    public CreateAddressRequestFaker()
    {
        RuleFor(request => request.City, faker => faker.Address.City());
        RuleFor(request => request.Street, faker => faker.Address.StreetAddress());
        RuleFor(request => request.Country, faker => faker.Address.Country());
    }
}