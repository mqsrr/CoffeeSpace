using System.Net;
using CoffeeSpace.PaymentService.Settings;
using Mediator;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Messages.Commands.Handlers;

internal sealed class CapturePaypalOrderCommandHandler : ICommandHandler<CapturePaypalOrderCommand, Order?>
{
    private readonly PayPalHttpClient _payPalHttpClient;
    
    public CapturePaypalOrderCommandHandler(IOptions<PaypalAuthenticationSettings> authSettings)
    {
        var paypalAuthSettings = authSettings.Value;
        var sendBoxEnvironment = new SandboxEnvironment(paypalAuthSettings.ClientId, paypalAuthSettings.ClientSecret);

        _payPalHttpClient = new PayPalHttpClient(sendBoxEnvironment);
    }
    
    public async ValueTask<Order?> Handle(CapturePaypalOrderCommand command, CancellationToken cancellationToken)
    {
        var request = new OrdersCaptureRequest(command.PaypalPaymentId);
        var requestBody = new OrderActionRequest();

        request.Prefer("return=representation");
        request.RequestBody(requestBody);

        var response = await _payPalHttpClient.Execute(request);
        if (response.StatusCode is not HttpStatusCode.Created)
        {
            return null;
        }

        var confirmedOrder = response.Result<Order>();
        return confirmedOrder;
    }
}