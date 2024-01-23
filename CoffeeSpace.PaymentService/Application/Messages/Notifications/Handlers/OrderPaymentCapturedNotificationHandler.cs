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
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICacheService _cacheService;

    public OrderPaymentCapturedNotificationHandler(IPublishEndpoint publishEndpoint, ICacheService  cacheService)
    {
        _publishEndpoint = publishEndpoint;
        _cacheService = cacheService;
    }

    public async ValueTask Handle(OrderPaymentCapturedNotification notification, CancellationToken cancellationToken)
    {
        var applicationOrder = await _cacheService.HashGetAsync<Order>(CacheKeys.Payments.HashKey, CacheKeys.Payments.GetById(notification.CapturedPaypalOrderId));
        await _publishEndpoint.Publish<OrderPaymentSuccess>(new
        {
            PaypalOrderInformationId = notification.CapturedPaypalOrderId,
            Order = applicationOrder
        }, context => context.RequestId = Guid.Parse(applicationOrder!.Id), cancellationToken);
    }
}