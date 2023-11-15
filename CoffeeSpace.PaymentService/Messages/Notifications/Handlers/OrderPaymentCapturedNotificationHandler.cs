using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Responses;
using MassTransit;
using Mediator;

namespace CoffeeSpace.PaymentService.Messages.Notifications.Handlers;

internal sealed class OrderPaymentCapturedNotificationHandler 
    : INotificationHandler<OrderPaymentCapturedNotification>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICacheService<Order> _cacheStack;

    public OrderPaymentCapturedNotificationHandler(IPublishEndpoint publishEndpoint, ICacheService<Order>  cacheStack)
    {
        _publishEndpoint = publishEndpoint;
        _cacheStack = cacheStack;
    }

    public async ValueTask Handle(OrderPaymentCapturedNotification notification, CancellationToken cancellationToken)
    {
        var applicationOrder = await _cacheStack.GetAsync($"payment-{notification.CapturedPaypalOrderId}", cancellationToken);
        await _publishEndpoint.Publish<OrderPaymentSuccess>(new
        {
            PaypalOrderInformationId = notification.CapturedPaypalOrderId,
            Order = applicationOrder
        }, context => context.RequestId = Guid.Parse(applicationOrder!.Id), cancellationToken);
    }
}