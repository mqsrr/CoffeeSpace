using System.Diagnostics.CodeAnalysis;
using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.Messages.Products.Events;
using CoffeeSpace.Messages.Products.Responses;
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

    public OrderStateMachine()
    {
        Event(() => SubmitOrder, x
            => x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));
        
        Event(() => AwaitProductsValidation, x
            => x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));
        
        Event(() => OrderStockValidationResult, x
            => x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));
        
        Event(() => OrderPaymentValidation, x
            => x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));
        
        Event(() => OrderPaymentValidationResult, x
            => x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));
        
        Event(() => ShipOrder, x
            => x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));
        
        Event(() => CancelOrder, x
            => x.CorrelateById(context => Guid.Parse(context.Message.Order.Id)));
        
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
                    context.Saga.StockValidationSuccess = false;
                    context.Saga.PaymentSuccess = false;
                })
                .PublishAsync(context => context.Init<AwaitProductsValidation>(new
                {
                    context.Message.Order,
                    Products = context.Message.Order.OrderItems.Select(x => x.ToProduct())
                }))
                .TransitionTo(Submitted));

        WhenEnter(Paid, binder => binder.Finalize());

        DuringAny(
            When(SubmitOrder)
                .TransitionTo(Submitted),
            When(AwaitProductsValidation)
                .TransitionTo(Submitted),
            When(OrderPaymentValidation)
                .TransitionTo(StockConfirmed),
            When(ShipOrder)
                .TransitionTo(Paid)
                .Finalize());

        DuringAny(
            When(OrderStockValidationResult)
                .IfElse(context => context.Message.IsValid, binder => 
                        binder.PublishAsync(context => context.Init<OrderPaymentValidation>(new
                            {
                                context.Message.Order
                            }))
                            .TransitionTo(StockConfirmed)
                            .Then(context => Console.WriteLine("{0} Order stock was validated", context.Message.Order.Id)), 
                        binder => 
                            binder.TransitionTo(Canceled)
                                .Then(context => Console.WriteLine("{0} Order was canceled due to invalid containing stock!", context.Message.Order.Id)))
                .Then(context => context.Saga.StockValidationSuccess = context.Message.IsValid),
            When(OrderPaymentValidationResult)
                .TransitionTo(Paid)
                .Then(context => context.Saga.PaymentSuccess = context.Message.IsValid));

        SetCompletedWhenFinalized();
    }

    public Event<SubmitOrder> SubmitOrder { get; private set; }
    
    public Event<AwaitProductsValidation> AwaitProductsValidation { get; private set; }
    
    public Event<OrderStockValidationResult> OrderStockValidationResult { get; private set; }
    
    public Event<OrderPaymentValidation> OrderPaymentValidation { get; private set; }
    
    public Event<OrderPaymentValidationResult> OrderPaymentValidationResult { get; private set; }
    
    public Event<ShipOrder> ShipOrder { get; private set; }
    
    public Event<CancelOrder> CancelOrder { get; private set; }
}