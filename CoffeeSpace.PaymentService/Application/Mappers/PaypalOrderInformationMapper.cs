using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.PaymentService.Application.Models;

namespace CoffeeSpace.PaymentService.Application.Mappers;

public static class PaypalOrderInformationMapper
{
    public static PaypalOrderInformation ToPaypalOrderInformation(this PayPalCheckoutSdk.Orders.Order paypalOrder, Order applicationOrder)
    {
        return new PaypalOrderInformation
        {
            Id = Guid.NewGuid(),
            ApplicationOrderId = applicationOrder.Id,
            BuyerId = applicationOrder.BuyerId,
            PaypalOrder = paypalOrder,
            PaypalOrderId = paypalOrder.Id
        };
    }
}