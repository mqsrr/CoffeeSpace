using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using Mediator;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Handlers;

public sealed class UpdateOrderStatusCommandHandler : ICommandHandler<UpdateOrderStatusCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublisher _publisher;
    private readonly IHubContext<OrderingHub, IOrderingHub> _hubContext;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IPublisher publisher, IHubContext<OrderingHub, IOrderingHub> hubContext)
    {
        _orderRepository = orderRepository;
        _publisher = publisher;
        _hubContext = hubContext;
    }
    public async ValueTask<Unit> Handle(UpdateOrderStatusCommand command, CancellationToken cancellationToken)
    {
        
        bool isStatusUpdated = await _orderRepository.UpdateOrderStatusAsync(command.OrderId, command.Status, cancellationToken);
        if (!isStatusUpdated)
        {
            return Unit.Value;
        }

        await _publisher.Publish(new UpdateOrderNotification
        {
            BuyerId = command.BuyerId,
            Id = command.OrderId
        }, cancellationToken);
            
        await _hubContext.Clients.Groups(command.BuyerId.ToString(), "Web Dashboard").OrderStatusUpdated(command.Status, command.OrderId);
        return Unit.Value;
    }
}