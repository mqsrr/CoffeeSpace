using Mediator;

namespace CoffeeSpace.PaymentService.Messages.Notifications;

public sealed class OrderPaymentCapturedNotification : INotification
{
    public required string CapturedPaypalOrderId { get; init; }
}