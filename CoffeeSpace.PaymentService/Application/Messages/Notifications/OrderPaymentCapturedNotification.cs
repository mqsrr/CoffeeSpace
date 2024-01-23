using Mediator;

namespace CoffeeSpace.PaymentService.Application.Messages.Notifications;

public sealed class OrderPaymentCapturedNotification : INotification
{
    public required string CapturedPaypalOrderId { get; init; }
}