using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands;
using MassTransit;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Activities;

internal sealed class UpdateOrderStatusActivity : IStateMachineActivity<OrderStateInstance>
{
    private readonly ISender _sender;

    public UpdateOrderStatusActivity(ISender sender)
    {
        _sender = sender;
    }

    public async Task Execute(BehaviorContext<OrderStateInstance> context, IBehavior<OrderStateInstance> next)
    {
        await _sender.Send(new UpdateOrderStatusCommand
        {
            OrderId = context.Saga.OrderId,
            BuyerId = context.Saga.BuyerId,
            Status = (OrderStatus)(context.Saga.CurrentState - 3)
        });
        
        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Execute<T>(BehaviorContext<OrderStateInstance, T> context, IBehavior<OrderStateInstance, T> next)
        where T : class
    {
        return next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderStateInstance, TException> context,
        IBehavior<OrderStateInstance> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<OrderStateInstance, T, TException> context,
        IBehavior<OrderStateInstance, T> next) where T : class where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("order-status-updated");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }
}