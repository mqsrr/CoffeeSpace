using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Payment.Responses;
using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Activities;
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
        {
            x.CorrelateById(context => context.Message.Order.Id);

            x.SetSagaFactory(context => new OrderStateInstance
            {
                CorrelationId = context.Message.Order.Id,
                Order = context.Message.Order,
                StockValidationSuccess = false,
                PaymentSuccess = false
            });
        });

        Event(() => CancelOrder, x =>
            x.CorrelateById(context => context.Message.Order.Id));

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
                .Request(RequestOrderStockValidation, context => context.Init<ValidateOrderStock>(new
                {
                    context.Message.Order.Id,
                     context.Message.Order.OrderItems
                }))
                .TransitionTo(Submitted));

        WhenEnterAny(binder => binder.If(context => context.Saga.CurrentState > 2,
            activityBinder => activityBinder.Activity(selector => selector.OfType<UpdateOrderStatusActivity>())));

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
                    logger.LogWarning("The order with ID {OrderId} has reached the timeout value for product validation", context.Saga.Order.Id))
                .TransitionTo(Canceled),
            When(RequestOrderStockValidation.Completed)
                .Request(RequestOrderPayment, context => context.Init<RequestOrderPayment>(new
                {
                    context.Saga.Order
                }))
                .Then(context => context.Saga.StockValidationSuccess = true)
                .TransitionTo(StockConfirmed));

        During(StockConfirmed,
            When(RequestOrderPayment.TimeoutExpired)
                .Then(context =>
                    logger.LogWarning("The order with ID {OrderId} has reached the timeout value for payment request", context.Saga.Order.Id))
                .TransitionTo(Canceled),
            When(RequestOrderPayment.Completed)
                .Request(RequestOrderShipment, context => context.Init<RequestOrderShipment>(new
                {
                    context.Saga.Order.Id,
                    context.Saga.Order.Address
                }))
                .Then(context => context.Saga.PaymentSuccess = true)
                .TransitionTo(Paid),
            Ignore(RequestOrderStockValidation.TimeoutExpired));

        During(Paid,
            When(RequestOrderShipment.TimeoutExpired)
                .Then(context =>
                    logger.LogWarning("The order with ID {OrderId} has reached the timeout value for shipment request", context.Saga.Order.Id))
                .TransitionTo(Canceled),
            When(RequestOrderShipment.Completed)
                .Activity(selector => selector.OfInstanceType<UpdateOrderStatusActivity>())
                .TransitionTo(Shipped)
                .Finalize(),
            Ignore(RequestOrderPayment.TimeoutExpired));

        SetCompletedWhenFinalized();
    }

    public required Event<SubmitOrder> SubmitOrder { get; init; }
    public required Event<CancelOrder> CancelOrder { get; init; }

    public Request<OrderStateInstance, RequestOrderPayment, OrderPaid> RequestOrderPayment { get; init; }
    public Request<OrderStateInstance, ValidateOrderStock, OrderStockConfirmed> RequestOrderStockValidation { get; init; }
    public Request<OrderStateInstance, RequestOrderShipment, OrderShipped> RequestOrderShipment { get; init; }
}