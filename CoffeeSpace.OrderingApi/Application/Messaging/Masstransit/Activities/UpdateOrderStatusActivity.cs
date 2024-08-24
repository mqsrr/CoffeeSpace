using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using MassTransit;
using Mediator;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Activities;

internal sealed class UpdateOrderStatusActivity : IStateMachineActivity<OrderStateInstance>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublisher _publisher;
    private readonly IHubContext<OrderingHub, IOrderingHub> _hubContext;

    public UpdateOrderStatusActivity(IOrderRepository orderRepository, IPublisher publisher,
        IHubContext<OrderingHub, IOrderingHub> hubContext)
    {
        _orderRepository = orderRepository;
        _publisher = publisher;
        _hubContext = hubContext;
    }

    public async Task Execute(BehaviorContext<OrderStateInstance> context, IBehavior<OrderStateInstance> next)
    {
        var orderToUpdate = context.Saga.Order;
        bool isStatusUpdated = await _orderRepository.UpdateOrderStatusAsync(
            orderToUpdate.Id, (OrderStatus)context.Saga.CurrentState - 3, context.CancellationToken);

        if (!isStatusUpdated)
        {
            await next.Execute(context).ConfigureAwait(false);
        }

        await _publisher.Publish(new UpdateOrderNotification
        {
            BuyerId = orderToUpdate.BuyerId,
            Id = orderToUpdate.Id
        }, context.CancellationToken);

        await _hubContext.Clients
            .Groups(orderToUpdate.BuyerId.ToString(), "Web Dashboard")
            .OrderStatusUpdated(orderToUpdate.Status, orderToUpdate.Id);

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