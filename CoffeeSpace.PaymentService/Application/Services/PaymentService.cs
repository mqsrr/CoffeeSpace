using System.Net;
using CoffeeSpace.Messages;
using CoffeeSpace.Messages.Payment.Responses;
using CoffeeSpace.PaymentService.Application.Extensions;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using CoffeeSpace.PaymentService.Application.Settings;
using CoffeeSpace.PaymentService.Models;
using MassTransit;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Order = CoffeeSpace.Domain.Ordering.Orders.Order;

namespace CoffeeSpace.PaymentService.Application.Services;

internal sealed class PaymentService : IPaymentService
{
    private readonly PayPalHttpClient _payPalHttpClient;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public PaymentService(IOptions<PaypalAuthenticationSettings> authSettings, ISendEndpointProvider sendEndpointProvider)
    {
        var paypalAuthSettings = authSettings.Value;
        var sendBoxEnvironment = new SandboxEnvironment(paypalAuthSettings.ClientId, paypalAuthSettings.ClientSecret);

        _sendEndpointProvider = sendEndpointProvider;
        _payPalHttpClient = new PayPalHttpClient(sendBoxEnvironment);
    }

    public async Task<PayPalCheckoutSdk.Orders.Order?> CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var request = new OrdersCreateRequest();
        var requestBody = new PayPalOrderRequestBuilder()
            .WithDefaultContext(order.Id)
            .WithPurchaseUnits(order.OrderItems.ToList())
            .WithAddress(order.Address)
            .Build();

        request.Headers.Add("prefer", "return=representation");
        request.RequestBody(requestBody);

        var response = await _payPalHttpClient.Execute(request);
        if (response.StatusCode is not HttpStatusCode.Created)
        {
            return null;
        }

        var createdOrder = response.Result<PayPalCheckoutSdk.Orders.Order>();
        return createdOrder;
    }

    public async Task CapturePaypalPaymentAsync(OrderApprovedWebhookEvent webhookEvent,
        CancellationToken cancellationToken)
    {
        var request = new OrdersCaptureRequest(webhookEvent.Resource.Id);
        var requestBody = new OrderActionRequest();

        request.Prefer("return=representation");
        request.RequestBody(requestBody);

        var response = await _payPalHttpClient.Execute(request);
        if (response.StatusCode is not HttpStatusCode.Created)
        {
            return;
        }

        var confirmedOrder = response.Result<PayPalCheckoutSdk.Orders.Order>();
        await _sendEndpointProvider.Send<OrderPaid>(
            new Uri(EndpointAddresses.Payment.OrderPaid),
            new { },
            context => context.RequestId = Guid.Parse(confirmedOrder.PurchaseUnits[0].ReferenceId), cancellationToken);
    }
}