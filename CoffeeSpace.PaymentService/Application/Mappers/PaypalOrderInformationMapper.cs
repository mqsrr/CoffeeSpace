using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.PaymentService.Application.Models;

namespace CoffeeSpace.PaymentService.Application.Mappers;

public static class PaypalOrderInformationMapper
{
    public static PaypalOrderInformation ToPaypalOrderInformation(this PayPalCheckoutSdk.Orders.Order paypalOrder, Guid applicationOrderId, Guid buyerId)
    {
        return new PaypalOrderInformation
        {
            Id = Guid.NewGuid(),
            ApplicationOrderId = applicationOrderId,
            BuyerId = buyerId,
            PaypalOrder = paypalOrder,
        };
    }
}