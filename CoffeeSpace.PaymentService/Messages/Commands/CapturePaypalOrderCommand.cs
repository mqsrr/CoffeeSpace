using Mediator;

namespace CoffeeSpace.PaymentService.Messages.Commands;

public sealed class CapturePaypalOrderCommand : ICommand<PayPalCheckoutSdk.Orders.Order?>
{
    public required string PaypalPaymentId { get; init; }
}