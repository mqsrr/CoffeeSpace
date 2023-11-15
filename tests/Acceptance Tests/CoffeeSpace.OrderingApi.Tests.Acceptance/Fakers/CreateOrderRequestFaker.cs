using AutoBogus;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Fakers;

internal sealed class CreateOrderRequestFaker : AutoFaker<CreateOrderRequest>
{
    public CreateOrderRequestFaker()
    {
        RuleFor(request => request.Address, AutoFaker.Generate<CreateAddressRequest, CreateAddressRequestFaker>());
        RuleFor(request => request.PaymentInfo, AutoFaker.Generate<CreatePaymentInfoRequest, CreatePaymentInfoRequestFaker>());
        RuleFor(request => request.Status, OrderStatus.Submitted);
        RuleFor(request => request.OrderItems, AutoFaker.Generate<CreateOrderItemRequest, CreateOrderItemRequestFaker>(3));
    }
}