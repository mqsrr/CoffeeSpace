using System.Net;
using CoffeeSpace.PaymentService.Extensions;
using CoffeeSpace.PaymentService.Settings;
using Mediator;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Messages.Commands.Handlers;

internal sealed class CreatePaypalOrderCommandHandler : ICommandHandler<CreatePaypalOrderCommand, Order?>
{
    private readonly PayPalHttpClient _payPalHttpClient;

    public CreatePaypalOrderCommandHandler(IOptions<PaypalAuthenticationSettings> authSettings)
    {
        var paypalAuthSettings = authSettings.Value;
        var sendBoxEnvironment = new SandboxEnvironment(paypalAuthSettings.ClientId, paypalAuthSettings.ClientSecret);

        _payPalHttpClient = new PayPalHttpClient(sendBoxEnvironment);
    }
    
    public async ValueTask<Order?> Handle(CreatePaypalOrderCommand command, CancellationToken cancellationToken)
    {
        var request = new OrdersCreateRequest();
        var requestBody = new PayPalOrderRequestBuilder()
            .WithDefaultContext()
            .WithPurchaseUnits(command.ApplicationOrder.OrderItems.ToList())
            .WithAddress(command.ApplicationOrder.Address)
            .Build();

        request.Headers.Add("prefer", "return=representation");
        request.RequestBody(requestBody);

        var response = await _payPalHttpClient.Execute(request);
        if (response.StatusCode is not HttpStatusCode.Created)
        {
            return null;
        }

        var createdOrder = response.Result<Order>();
        return createdOrder;
    }
}