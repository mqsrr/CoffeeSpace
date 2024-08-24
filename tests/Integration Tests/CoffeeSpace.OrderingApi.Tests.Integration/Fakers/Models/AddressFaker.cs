using AutoBogus;
using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;

public sealed class AddressFaker : AutoFaker<Address>
{
    public AddressFaker()
    {
        RuleFor(order => order.Id, faker => faker.Random.Guid());
        RuleFor(order => order.City, faker => faker.Address.City());
        RuleFor(order => order.Country, faker => faker.Address.Country());
        RuleFor(order => order.Street, faker => faker.Address.StreetAddress());
    }
}