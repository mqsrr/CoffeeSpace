using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;

public sealed class BuyerFaker : AutoFaker<Buyer>
{
    public BuyerFaker()
    {
        RuleFor(buyer => buyer.Id, faker => faker.Random.Guid());
        RuleFor(buyer => buyer.Email, faker => faker.Person.Email);
        RuleFor(buyer => buyer.Name, faker => faker.Person.FirstName);
    }
}