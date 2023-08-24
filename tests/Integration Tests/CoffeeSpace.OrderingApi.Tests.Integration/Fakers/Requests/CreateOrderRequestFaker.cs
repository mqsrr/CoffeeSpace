using AutoBogus;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

public sealed class CreateOrderRequestFaker : AutoFaker<CreateOrderRequest>
{
    public CreateOrderRequestFaker()
    {
        UseSeed(69);
    }
}