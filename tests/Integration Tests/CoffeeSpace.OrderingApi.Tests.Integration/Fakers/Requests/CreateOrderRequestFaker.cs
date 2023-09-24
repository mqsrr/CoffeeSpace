using AutoBogus;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.PaymentInfo;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

internal sealed class CreateOrderRequestFaker : AutoFaker<CreateOrderRequest>
{
    public CreateOrderRequestFaker()
    {
        RuleFor(request => request.Address, AutoFaker.Generate<CreateAddressRequest, CreateAddressRequestFaker>());
        RuleFor(request => request.PaymentInfo, AutoFaker.Generate<CreatePaymentInfoRequest, CreatePaymentInfoRequestFaker>());
        RuleFor(request => request.Status, OrderStatus.Submitted);
    }
}