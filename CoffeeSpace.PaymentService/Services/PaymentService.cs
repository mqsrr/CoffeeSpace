﻿using CoffeeSpace.PaymentService.Messages.Commands;
using CoffeeSpace.PaymentService.Messages.Queries;
using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.PaymentService.Services.Abstractions;
using Mediator;
using Order = CoffeeSpace.Domain.Ordering.Orders.Order;

namespace CoffeeSpace.PaymentService.Services;

internal sealed class PaymentService : IPaymentService
{
    private readonly ISender _sender;
    
    public PaymentService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<PayPalCheckoutSdk.Orders.Order?> CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var createdPaypalOrder = await _sender.Send(new CreatePaypalOrderCommand
        {
            ApplicationOrder = order
        }, cancellationToken);

        return createdPaypalOrder;
    }

    public async Task<PayPalCheckoutSdk.Orders.Order?> CapturePaypalPaymentAsync(string paypalPaymentId, 
        CancellationToken cancellationToken)
    {
        var capturedPaypalOrder = await _sender.Send(new CapturePaypalOrderCommand
        {
            PaypalPaymentId = paypalPaymentId
        }, cancellationToken);

        return capturedPaypalOrder;
    }

    public async Task<PaypalOrderInformation?> GetOrderPaymentByOrderIdAsync(string applicationOrderId, 
        CancellationToken cancellationToken)
    {
        var paypalOrderInformation = await _sender.Send(new GetPaypalOrderInformationByOrderIdQuery
        {
            Id = applicationOrderId
        }, cancellationToken);

       return paypalOrderInformation;
    }

}