using AutoBogus;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

public sealed class UpdateBuyerRequestFaker : AutoFaker<UpdateBuyerRequest>
{
    public UpdateBuyerRequestFaker()
    {
        RuleFor(request => request.Name, faker => faker.Person.FirstName);
        RuleFor(request => request.Email, faker => faker.Person.Email);
    }
}