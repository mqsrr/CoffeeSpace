using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.OrderingApi.Application.Mapping;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using MassTransit;
using Mediator;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders.Handlers;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IHubContext<OrderingHub, IOrderingHub> _hubContext;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint, IHubContext<OrderingHub, IOrderingHub> hubContext)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
        _hubContext = hubContext;
    }

    public async ValueTask<bool> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        bool created = await _orderRepository.CreateAsync(command.Order, cancellationToken);
        if (created)  
        {
            await _publishEndpoint.Publish<SubmitOrder>(new
            {
                command.Order
            }, cancellationToken);

            await _hubContext.Clients.Groups(command.Order.BuyerId, "Web Dashboard").OrderCreated(command.Order.ToResponse());
        }
        
        return created;
    }
}