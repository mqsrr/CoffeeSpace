using AutoBogus;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

internal sealed class FaultCreateBuyerRequest : AutoFaker<CreateBuyerRequest>
{
    public FaultCreateBuyerRequest()
    {
        RuleFor(request => request.Email, string.Empty);
        RuleFor(request => request.Name, string.Empty);
    }
}