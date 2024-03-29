using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.Messages.Payment;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using MassTransit;

namespace CoffeeSpace.PaymentService.Application.Messages.Consumers;

internal sealed class OrderPaymentValidationConsumer : IConsumer<RequestOrderPayment>
{
    private readonly IPaymentService _paymentService;
    private readonly ITopicProducerProvider _topicProducerProvider;

    public OrderPaymentValidationConsumer(IPaymentService paymentService, ITopicProducerProvider topicProducerProvider)
    {
        _paymentService = paymentService;
        _topicProducerProvider = topicProducerProvider;
    }

    public async Task Consume(ConsumeContext<RequestOrderPayment> context)
    {
        var createdOrder = await _paymentService.CreateOrderAsync(context.Message.Order, context.CancellationToken);
        if (createdOrder is null)
        {
            var faultTopicEndpoint = _topicProducerProvider.GetProducer<Fault<RequestOrderPayment>>(new Uri("topic:order-payment-failed"));
            await faultTopicEndpoint.Produce(new
            {
                context.Message
            }, context.CancellationToken);
            
            return;
        }

        var topicEndpoint = _topicProducerProvider.GetProducer<PaymentPageInitialized>(new Uri("topic:order-payment-initialized"));
        await topicEndpoint.Produce(new
        {
            OrderId = context.Message.Order.Id,
            context.Message.Order.BuyerId,
            PaymentApprovalLink = createdOrder.Links[1].Href
        }, context.CancellationToken);
    }
}