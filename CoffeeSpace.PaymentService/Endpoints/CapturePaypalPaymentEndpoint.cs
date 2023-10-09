using CoffeeSpace.PaymentService.Contracts.Requests;
using CoffeeSpace.PaymentService.Helpers;
using CoffeeSpace.PaymentService.Services.Abstractions;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.PaymentService.Endpoints;

[FastEndpoints.HttpGet(ApiEndpoints.Payments.CapturePaypalPayment)]
[AllowAnonymous]
internal sealed class CapturePaypalPaymentEndpoint : Endpoint<CapturePaypalPaymentRequest, Results<Ok<PayPalCheckoutSdk.Orders.Order>, BadRequest>>
{
    private readonly IPaymentService _paymentService;

    public CapturePaypalPaymentEndpoint(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public override async Task<Results<Ok<PayPalCheckoutSdk.Orders.Order>, BadRequest>> ExecuteAsync([FromQuery]CapturePaypalPaymentRequest req, CancellationToken ct)
    {
        var capturedOrder = await _paymentService.CapturePaypalPaymentAsync(req.Token, ct);
        return capturedOrder is not null
            ? TypedResults.Ok(capturedOrder)
            : TypedResults.BadRequest();
    }
}