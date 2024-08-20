using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Payment.Commands;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using MassTransit;

namespace CoffeeSpace.PaymentService.Application.Messages.Consumers;

internal sealed class OrderPaymentValidationConsumer : IConsumer<RequestOrderPayment>
{
    private readonly IPaymentService _paymentService;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public OrderPaymentValidationConsumer(IPaymentService paymentService, ISendEndpointProvider sendEndpointProvider)
    {
        _paymentService = paymentService;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Consume(ConsumeContext<RequestOrderPayment> context)
    {
        var createdOrder = await _paymentService.CreateOrderAsync(context.Message.Order, context.CancellationToken);
        if (createdOrder is null)
        {
            await context.RespondAsync<Fault<RequestOrderPayment>>(context.Message);
            return;
        }

        await _sendEndpointProvider.Send<PaymentPageInitialized>(new
        {
            OrderId = context.Message.Order.Id,
            context.Message.Order.BuyerId,
            PaymentApprovalLink = createdOrder.Links[1].Href
        });
    }
}