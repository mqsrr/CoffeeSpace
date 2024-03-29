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
    private readonly ITopicProducer<OrderPaid> _topicProducer;
    private readonly ICacheService _cacheService;

    public OrderPaymentCapturedNotificationHandler(ITopicProducer<OrderPaid> topicProducer, ICacheService  cacheService)
    {
        _topicProducer = topicProducer;
        _cacheService = cacheService;
    }

    public async ValueTask Handle(OrderPaymentCapturedNotification notification, CancellationToken cancellationToken)
    {
        var applicationOrder = await _cacheService.GetAsync<Order>(CacheKeys.Payments.GetById(notification.CapturedPaypalOrderId), cancellationToken);
        await _topicProducer.Produce(new
        {
            PaypalOrderInformationId = notification.CapturedPaypalOrderId,
            Order = applicationOrder
        }, cancellationToken);
    }
}