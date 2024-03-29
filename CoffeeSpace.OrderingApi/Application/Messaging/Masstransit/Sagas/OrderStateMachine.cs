using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Activities;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Configurations;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;

internal sealed class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    public required State Submitted { get; init; }

    public required State StockConfirmed { get; init; }

    public required State Paid { get; init; }

    public required State Shipped { get; init; }

    public required State Canceled { get; init; }

    public OrderStateMachine()
    {
        MessageCorrelationConfiguration.ConfigureStateMachineMessages();
        
        Event(() => SubmitOrder);
        Event(() => CancelOrder);

        Event(() => RequestOrderStockValidation);
        Event(() => OrderStockConfirmed);
        Event(() => FailedToConfirmOrderStock);

        Event(() => RequestOrderPayment);
        Event(() => OrderPaid);
        Event(() => FailedToRequestOrderPayment);

        Event(() => RequestOrderShipment);
        Event(() => OrderShipped);
        Event(() => FailedToRequestOrderShipment);

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
                .TransitionTo(Submitted)
                .Produce(context => context.Init<ValidateOrderStock>(new
                {
                    context.Message.Order,
                    ProductTitles = context.Message.Order.OrderItems.Select(item => item.Title)
                }))
        );

        WhenEnterAny(binder => binder.If(context => context.Saga.CurrentState > 2, 
            activityBinder => activityBinder.Activity(selector => selector.OfType<UpdateOrderStatusActivity>())));

        WhenEnter(Canceled, binder => binder.Finalize());

        DuringAny(
            When(FailedToConfirmOrderStock)
                .TransitionTo(Canceled),
            When(FailedToRequestOrderPayment)
                .TransitionTo(Canceled),
            When(FailedToRequestOrderShipment)
                .TransitionTo(Canceled));

        During(Submitted,
            When(OrderStockConfirmed)
                .Produce(context => context.Init<RequestOrderPayment>(new
                {
                    context.Message.Order
                }))
                .TransitionTo(StockConfirmed)
                .Then(context => context.Saga.StockValidationSuccess = true));

        During(StockConfirmed,
            When(OrderPaid)
                .Produce(context => context.Init<RequestOrderShipment>(new
                {
                    context.Message.Order
                }))
                .TransitionTo(Paid)
                .Then(context => context.Saga.PaymentSuccess = true));

        During(Paid,
            When(OrderShipped)
                .Activity(selector => selector.OfInstanceType<UpdateOrderStatusActivity>())
                .TransitionTo(Shipped)
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public required Event<SubmitOrder> SubmitOrder { get; init; }
    public required Event<CancelOrder> CancelOrder { get; init; }
    
    public required Event<ValidateOrderStock> RequestOrderStockValidation { get; init; }
    public required Event<OrderStockConfirmed> OrderStockConfirmed { get; init; }
    public required Event<Fault<ValidateOrderStock>> FailedToConfirmOrderStock { get; init; }

    public required Event<RequestOrderPayment> RequestOrderPayment { get; init; }
    public required Event<OrderPaid> OrderPaid { get; init; }
    public required Event<Fault<RequestOrderPayment>> FailedToRequestOrderPayment { get; init; }

    public required Event<RequestOrderShipment> RequestOrderShipment { get; init; }
    public required Event<OrderShipped> OrderShipped { get; init; }
    public required Event<Fault<RequestOrderShipment>> FailedToRequestOrderShipment { get; init; }
}