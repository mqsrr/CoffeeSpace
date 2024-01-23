using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;

internal sealed class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    public required State Submitted { get; init; }

    public required State StockConfirmed { get; init; }

    public required State Paid { get; init; }

    public required State Shipped { get; init; }

    public required State Canceled { get; init; }

    public OrderStateMachine(ILogger<OrderStateMachine> logger)
    {
        Event(() => SubmitOrder, x =>
            x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));

        Event(() => CancelOrder, x =>
            x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));

        Request(() => RequestOrderStockValidation, x =>
            x.Timeout = TimeSpan.FromSeconds(60));

        Request(() => RequestOrderPayment, x =>
            x.Timeout = TimeSpan.FromMinutes(10));

        Request(() => RequestOrderShipment, x =>
            x.Timeout = TimeSpan.FromSeconds(60));

        InstanceState(x => x.CurrentState,
            Submitted,
            StockConfirmed,
            Paid,
            Shipped,
            Canceled);

        Initially(
            When(SubmitOrder)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.Order.Id;
                    context.Saga.BuyerId = context.Message.Order.BuyerId;
                    context.Saga.UpdateOrderStatusCorrelationId = context.Message.Order.Id;
                    context.Saga.StockValidationSuccess = false;
                    context.Saga.PaymentSuccess = false;
                })
                .Request(RequestOrderStockValidation, context => context.Init<OrderStockValidation>(new
                {
                    context.Message.Order,
                    ProductTitles = context.Message.Order.OrderItems.Select(item => item.Title)
                }))
                .TransitionTo(Submitted));

        WhenEnterAny(binder => binder.If(context => context.Saga.CurrentState > 2, activityBinder =>
            activityBinder.PublishAsync(context =>
                context.Init<UpdateOrderStatus>(new
                {
                    context.Saga.OrderId,
                    context.Saga.BuyerId,
                    Status = context.Saga.CurrentState - 3
                }))));

        WhenEnter(Canceled, binder => binder.Finalize());

        DuringAny(
            When(RequestOrderStockValidation!.Faulted)
                .TransitionTo(Canceled),
            When(RequestOrderPayment!.Faulted)
                .TransitionTo(Canceled),
            When(RequestOrderShipment!.Faulted)
                .TransitionTo(Canceled));

        During(Submitted,
            When(RequestOrderStockValidation.TimeoutExpired)
                .Then(context =>
                    logger.LogWarning("The order with ID {OrderId} has reached the timeout value for product validation",
                        context.Saga.OrderId))
                .TransitionTo(Canceled),
            When(RequestOrderStockValidation.Completed)
                .Request(RequestOrderPayment, context => context.Init<RequestOrderPayment>(new
                {
                    context.Message.Order
                }))
                .Then(context => context.Saga.StockValidationSuccess = true)
                .TransitionTo(StockConfirmed));

        During(StockConfirmed,
            When(RequestOrderPayment.TimeoutExpired)
                .Then(context =>
                    logger.LogWarning("The order with ID {OrderId} has reached the timeout value for payment request",
                        context.Saga.OrderId))
                .TransitionTo(Canceled),
            When(RequestOrderPayment.Completed)
                .Request(RequestOrderShipment, context => context.Init<RequestOrderShipment>(new
                {
                    context.Message.Order
                }))
                .Then(context => context.Saga.PaymentSuccess = true)
                .TransitionTo(Paid),
            Ignore(RequestOrderStockValidation.TimeoutExpired));

        During(Paid,
            When(RequestOrderShipment.TimeoutExpired)
                .Then(context =>
                    logger.LogWarning("The order with ID {OrderId} has reached the timeout value for shipment request",
                        context.Saga.OrderId))
                .TransitionTo(Canceled),
            When(RequestOrderShipment.Completed)
                .PublishAsync(context => context.Init<UpdateOrderStatus>(new
                {
                    context.Saga.OrderId,
                    context.Saga.BuyerId,
                    Status = OrderStatus.Shipped
                }))
                .TransitionTo(Shipped)
                .Finalize(),
            Ignore(RequestOrderPayment.TimeoutExpired));

        SetCompletedWhenFinalized();
    }

    public required Event<SubmitOrder> SubmitOrder { get; init; }

    public required Event<CancelOrder> CancelOrder { get; init; }

    public Request<OrderStateInstance, RequestOrderPayment, OrderPaymentSuccess> RequestOrderPayment { get; init; }

    public Request<OrderStateInstance, OrderStockValidation, OrderStockConfirmed> RequestOrderStockValidation { get; init; }

    public Request<OrderStateInstance, RequestOrderShipment, OrderShipped> RequestOrderShipment { get; init; }
}