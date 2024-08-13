using System.Net;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages;
using CoffeeSpace.Messages.Payment.Responses;
using CoffeeSpace.PaymentService.Application.Extensions;
using CoffeeSpace.PaymentService.Application.Mappers;
using CoffeeSpace.PaymentService.Application.Models;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using CoffeeSpace.PaymentService.Application.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Order = CoffeeSpace.Domain.Ordering.Orders.Order;

namespace CoffeeSpace.PaymentService.Application.Services;

internal sealed class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly PayPalHttpClient _payPalHttpClient;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public PaymentService(IOptions<PaypalAuthenticationSettings> authSettings, IPaymentRepository paymentRepository, ISendEndpointProvider sendEndpointProvider)
    {
        var paypalAuthSettings = authSettings.Value;
        var sendBoxEnvironment = new SandboxEnvironment(paypalAuthSettings.ClientId, paypalAuthSettings.ClientSecret);

        _paymentRepository = paymentRepository;
        _sendEndpointProvider = sendEndpointProvider;
        _payPalHttpClient = new PayPalHttpClient(sendBoxEnvironment);
    }

    public async Task<PayPalCheckoutSdk.Orders.Order?> CreateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var request = new OrdersCreateRequest();
        var requestBody = new PayPalOrderRequestBuilder()
            .WithDefaultContext()
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
        var paypalOrderInformation = createdOrder.ToPaypalOrderInformation(order.Id, order.BuyerId);

        await _paymentRepository.CreatePaymentAsync(paypalOrderInformation, cancellationToken);
        return createdOrder;
    }

    public async Task<PayPalCheckoutSdk.Orders.Order?> CapturePaypalPaymentAsync(string paypalPaymentId, CancellationToken cancellationToken)
    {
        var request = new OrdersCaptureRequest(paypalPaymentId);
        var requestBody = new OrderActionRequest();

        request.Prefer("return=representation");
        request.RequestBody(requestBody);

        var response = await _payPalHttpClient.Execute(request);
        if (response.StatusCode is not HttpStatusCode.Created)
        {
            return null;
        }

        var confirmedOrder = response.Result<PayPalCheckoutSdk.Orders.Order>();
        var paypalOrderInformation = await _paymentRepository.GetPaypalOrderByIdAsync(Guid.Parse(confirmedOrder.Id), cancellationToken);
        
        await _paymentRepository.UpdatePaymentStatusAsync(confirmedOrder.Id, confirmedOrder.Status, cancellationToken);
        await _sendEndpointProvider.Send<OrderPaid>(new Uri(EndpointAddresses.Payment.OrderPaid), new { },
            context => context.RequestId = paypalOrderInformation!.ApplicationOrderId, cancellationToken).ConfigureAwait(false);

        return confirmedOrder;
    }

    public Task<PaypalOrderInformation?> GetOrderPaymentByOrderIdAsync(Guid applicationOrderId,
        CancellationToken cancellationToken)
    {
        var paypalOrderInformation = _paymentRepository.GetByApplicationOrderIdAsync(applicationOrderId, cancellationToken);
        return paypalOrderInformation;
    }

}