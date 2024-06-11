using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.PaymentService.Application.Helpers;
using CoffeeSpace.Shared.Services.Abstractions;
using MassTransit;
using Mediator;

namespace CoffeeSpace.PaymentService.Application.Messages.Notifications.Handlers;

internal sealed class OrderPaymentCapturedNotificationHandler 
    : INotificationHandler<OrderPaymentCapturedNotification>
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly ICacheService _cacheService;

    public OrderPaymentCapturedNotificationHandler(ISendEndpointProvider sendEndpointProvider, ICacheService  cacheService)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _cacheService = cacheService;
    }

    public async ValueTask Handle(OrderPaymentCapturedNotification notification, CancellationToken cancellationToken)
    {
        var applicationOrder = await _cacheService.GetAsync<Order>(CacheKeys.Payments.GetById(notification.CapturedPaypalOrderId), cancellationToken);
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-state-instance"));
        
        await sendEndpoint.Send<OrderPaid>(new
        {
            PaypalOrderInformationId = notification.CapturedPaypalOrderId,
            Order = applicationOrder
        }, context => context.RequestId = applicationOrder!.Id, cancellationToken).ConfigureAwait(false);
    }
}