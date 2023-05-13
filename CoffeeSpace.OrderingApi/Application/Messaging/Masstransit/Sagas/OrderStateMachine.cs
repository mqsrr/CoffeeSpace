using System.Diagnostics.CodeAnalysis;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.Messages.Products.Events;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.Messages.Shipment.Events;
using CoffeeSpace.Messages.Shipment.Responses;
using CoffeeSpace.OrderingApi.Mapping;
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
            x.Timeout = TimeSpan.Zero);
               
        Request(() => RequestOrderPaymentValidation, x => 
            x.Timeout = TimeSpan.Zero);
                
        Request(() => RequestOrderShipment, x => 
            x.Timeout = TimeSpan.Zero);
        
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
                .Request(RequestOrderStockValidation,context => context.Init<AwaitProductsValidation>(new
                {
                    context.Message.Order,
                    Products = context.Message.Order.OrderItems.Select(x => x.ToProduct())
                }))
                .TransitionTo(Submitted));
        
        WhenEnterAny(binder => 
            binder.Then(context => 
                logger.LogInformation("{OrderId}-Order has been entered in a new state-{State}", 
                    context.Saga.OrderId,
                    context.Saga.CurrentState))
                .If(context => context.Saga.CurrentState > 2,
                    activityBinder => 
                        activityBinder.PublishAsync(context => context.Init<UpdateOrderStatus>(new
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
            When(RequestOrderStockValidation.Completed)
                .Request(RequestOrderPaymentValidation, context => context.Init<OrderPaymentValidation>(new
                {
                    context.Message.Order
                }))
                .Then(context => context.Saga.StockValidationSuccess = true)
                .TransitionTo(StockConfirmed),
            
            When(RequestOrderPaymentValidation.Completed)
                .Request(RequestOrderShipment, context => context.Init<RequestOrderShipment>(new
                {
                    context.Message.Order
                }))
                .Then(context => context.Saga.PaymentSuccess = true)
                .TransitionTo(Paid),
            
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

    public Request<OrderStateInstance, AwaitProductsValidation, OrderStockValidationResult> RequestOrderStockValidation { get; private set; }

    public Request<OrderStateInstance, OrderPaymentValidation, OrderPaymentValidationResult> RequestOrderPaymentValidation { get; private set; }
    
    public Request<OrderStateInstance, RequestOrderShipment, OrderShipmentResponse> RequestOrderShipment { get; private set; }
    
}