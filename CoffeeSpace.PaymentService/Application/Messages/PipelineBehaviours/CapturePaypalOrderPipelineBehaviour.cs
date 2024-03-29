using CoffeeSpace.PaymentService.Application.Messages.Commands;
using CoffeeSpace.PaymentService.Application.Messages.Notifications;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using Mediator;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Application.Messages.PipelineBehaviours;

internal sealed class CapturePaypalOrderPipelineBehaviour : IPipelineBehavior<CapturePaypalOrderCommand, Order?>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPublisher _publisher;

    public CapturePaypalOrderPipelineBehaviour(IPaymentRepository paymentRepository, IPublisher publisher)
    {
        _paymentRepository = paymentRepository;
        _publisher = publisher;
    }

    public async ValueTask<Order?> Handle(CapturePaypalOrderCommand message, CancellationToken cancellationToken,
        MessageHandlerDelegate<CapturePaypalOrderCommand, Order?> next)
    {
        var capturedPaypalOrder = await next(message, cancellationToken);
        if (capturedPaypalOrder is null)
        {
            return null;
        }
        
        await _paymentRepository.UpdatePaymentStatusAsync(capturedPaypalOrder.Id, capturedPaypalOrder.Status, cancellationToken);
        await _publisher.Publish(new OrderPaymentCapturedNotification
        {
            CapturedPaypalOrderId = capturedPaypalOrder.Id
        }, cancellationToken);
        
        return capturedPaypalOrder;
    }
}