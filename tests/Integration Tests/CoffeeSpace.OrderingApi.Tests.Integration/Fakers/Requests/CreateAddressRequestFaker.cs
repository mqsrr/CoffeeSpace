using AutoBogus;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

internal sealed class CreateAddressRequestFaker : AutoFaker<CreateAddressRequest>
{
    public CreateAddressRequestFaker()
    {
        RuleFor(request => request.City, faker => faker.Address.City());
        RuleFor(request => request.Street, faker => faker.Address.StreetAddress());
        RuleFor(request => request.Country, faker => faker.Address.Country());
    }
}