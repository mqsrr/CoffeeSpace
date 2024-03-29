using CoffeeSpace.Messages.Payment;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;

public sealed class PaymentPageInitializedConsumer : IConsumer<PaymentPageInitialized>
{
    private readonly IHubContext<OrderingHub, IOrderingHub> _hubContext;

    public PaymentPageInitializedConsumer(IHubContext<OrderingHub, IOrderingHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<PaymentPageInitialized> context)
    {
        await _hubContext.Clients
            .Group(context.Message.BuyerId.ToString())
            .OrderPaymentPageInitialized(context.Message.OrderId, context.Message.PaymentApprovalLink);
    }
}