using AutoBogus;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

internal sealed class CreateOrderRequestFaker : AutoFaker<CreateOrderRequest>
{
    public CreateOrderRequestFaker()
    {
        RuleFor(request => request.Address, () => AutoFaker.Generate<CreateAddressRequest, CreateAddressRequestFaker>());
        RuleFor(request => request.OrderItems, () => AutoFaker.Generate<OrderItem, OrderItemFaker>(5));
        RuleFor(request => request.Status, OrderStatus.Submitted);
    }
}