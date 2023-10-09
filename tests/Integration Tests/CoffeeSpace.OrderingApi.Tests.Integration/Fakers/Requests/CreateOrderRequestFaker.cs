using AutoBogus;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

internal sealed class CreateOrderRequestFaker : AutoFaker<CreateOrderRequest>
{
    public CreateOrderRequestFaker()
    {
        RuleFor(request => request.Address, AutoFaker.Generate<CreateAddressRequest, CreateAddressRequestFaker>());
        RuleFor(request => request.Status, OrderStatus.Submitted);
    }
}