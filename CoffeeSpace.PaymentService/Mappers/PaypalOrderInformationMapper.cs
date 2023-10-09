﻿using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.PaymentService.Models;

namespace CoffeeSpace.PaymentService.Mappers;

public static class PaypalOrderInformationMapper
{
    public static PaypalOrderInformation ToPaypalOrderInformation(this PayPalCheckoutSdk.Orders.Order paypalOrder, Order applicationOrder)
    {
        return new PaypalOrderInformation
        {
            Id = Guid.NewGuid().ToString(),
            ApplicationOrderId = applicationOrder.Id,
            BuyerId = applicationOrder.BuyerId,
            PaypalOrder = paypalOrder,
            PaypalOrderId = paypalOrder.Id
        };
    }
}