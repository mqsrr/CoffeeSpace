using AutoBogus;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

public sealed class CreateBuyerRequestFaker : AutoFaker<CreateBuyerRequest>
{
    public CreateBuyerRequestFaker()
    {
        RuleFor(request => request.Email, faker => faker.Person.Email);
        RuleFor(request => request.Name, faker => faker.Person.FirstName);
    }
}