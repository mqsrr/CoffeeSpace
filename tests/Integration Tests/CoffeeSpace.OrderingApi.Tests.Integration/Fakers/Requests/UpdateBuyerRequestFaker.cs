using AutoBogus;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

public sealed class UpdateBuyerRequestFaker : AutoFaker<UpdateBuyerRequest>
{
    public UpdateBuyerRequestFaker()
    {
        UseSeed(70);
        RuleFor(request => request.Id, faker => faker.Random.Guid());
        RuleFor(request => request.Name, faker => faker.Person.Email);
        RuleFor(request => request.Email, faker => faker.Person.FirstName);
    }
}