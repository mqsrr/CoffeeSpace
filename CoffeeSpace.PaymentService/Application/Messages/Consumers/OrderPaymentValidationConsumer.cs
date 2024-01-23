using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Payment;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using MassTransit;

namespace CoffeeSpace.PaymentService.Application.Messages.Consumers;

internal sealed class OrderPaymentValidationConsumer : IConsumer<RequestOrderPayment>
{
    private readonly IPaymentService _paymentService;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderPaymentValidationConsumer(IPaymentService paymentService, IPublishEndpoint publishEndpoint)
    {
        _paymentService = paymentService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<RequestOrderPayment> context)
    {
        var createdOrder = await _paymentService.CreateOrderAsync(context.Message.Order, context.CancellationToken);
        if (createdOrder is null)
        {
            await context.RespondAsync<Fault<RequestOrderPayment>>(context.Message);
            return;
        }

        await _publishEndpoint.Publish<PaymentPageInitialized>(new
        {
            OrderId = context.Message.Order.Id,
            context.Message.Order.BuyerId,
            PaymentApprovalLink = createdOrder.Links[1].Href
        });
    }
}