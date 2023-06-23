using System.Diagnostics.CodeAnalysis;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.Messages.Products.Events;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.Messages.Shipment.Events;
using CoffeeSpace.Messages.Shipment.Responses;
using CoffeeSpace.OrderingApi.Application.Mapping;
using MassTransit;

#pragma warning disable CS8618

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
internal sealed class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    public State Submitted { get; private set; }
    
    public State StockConfirmed { get; private set; }
    
    public State Paid { get; private set; }
    
    public State Shipped { get; private set; }
    
    public State Canceled { get; private set; }

    public OrderStateMachine(ILogger<OrderStateMachine> logger)
    {
        Event(() => SubmitOrder, x =>
            x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));

        Event(() => CancelOrder, x =>
            x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));

        Request(() => RequestOrderStockValidation, x =>
            x.Timeout = TimeSpan.FromSeconds(60));

        Request(() => RequestOrderPaymentValidation, x =>
            x.Timeout = TimeSpan.FromSeconds(60));

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
                    context.Saga.UpdateOrderStatudCorrelationId = context.Message.Order.Id;
                    context.Saga.BuyerId = context.Message.Order.BuyerId;
                    context.Saga.StockValidationSuccess = false;
                    context.Saga.PaymentSuccess = false;
                })
                .Request(RequestOrderStockValidation, context => context.Init<OrderStockValidation>(new
                {
                    context.Message.Order,
                    Products = context.Message.Order.OrderItems.Select(x => x.ToProduct())
                }))
                .TransitionTo(Submitted));

        WhenEnterAny(binder =>
            binder.Then(context =>
                {
                    logger.LogDebug("The order with ID {OrderId} has entered a new state: {State}",
                        context.Saga.OrderId,
                        context.Saga.CurrentState.ToString());
                })
                .If(context => context.Saga.CurrentState > 2, activityBinder =>
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
            When(RequestOrderPaymentValidation!.Faulted)
                .TransitionTo(Canceled),
            When(RequestOrderShipment!.Faulted)
                .TransitionTo(Canceled));

        DuringAny(
            When(RequestOrderStockValidation.TimeoutExpired)
                .Then(context => logger.LogWarning("The order with ID {OrderId} has reached the timeout value for product validation", context.Saga.OrderId))
                .TransitionTo(Canceled),
            When(RequestOrderPaymentValidation.TimeoutExpired)
                .Then(context => logger.LogWarning("The order with ID {OrderId} has reached the timeout value for payment validation", context.Saga.OrderId))
                .TransitionTo(Canceled),
            When(RequestOrderShipment.TimeoutExpired)
                .Then(context => logger.LogWarning("The order with ID {OrderId} has reached the timeout value for shipment request", context.Saga.OrderId))
                .TransitionTo(Canceled));
            
        During(Submitted,
            When(RequestOrderStockValidation.Completed)
                .Request(RequestOrderPaymentValidation, context => context.Init<OrderPaymentValidation>(new
                {
                    context.Message.Order
                }))
                .Then(context => context.Saga.StockValidationSuccess = true)
                .TransitionTo(StockConfirmed));

        During(StockConfirmed,
            When(RequestOrderPaymentValidation.Completed)
                .Request(RequestOrderShipment, context => context.Init<RequestOrderShipment>(new
                {
                    context.Message.Order
                }))
                .Then(context => context.Saga.PaymentSuccess = true)
                .TransitionTo(Paid));

        During(Paid,
            When(RequestOrderShipment.Completed)
                .PublishAsync(context => context.Init<UpdateOrderStatus>(new
                {
                    context.Saga.OrderId,
                    context.Saga.BuyerId,
                    Status = OrderStatus.Shipped
                }))
                .TransitionTo(Shipped)
                .Finalize());


        SetCompletedWhenFinalized();
    }

    public Event<SubmitOrder> SubmitOrder { get; private set; }
    
    public Event<CancelOrder> CancelOrder { get; private set; }

    public Request<OrderStateInstance, OrderStockValidation, OrderStockValidationResult> RequestOrderStockValidation { get; private set; }

    public Request<OrderStateInstance, OrderPaymentValidation, OrderPaymentValidationResult> RequestOrderPaymentValidation { get; private set; }
    
    public Request<OrderStateInstance, RequestOrderShipment, OrderShipmentResponse> RequestOrderShipment { get; private set; }
}