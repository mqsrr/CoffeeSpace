using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using MassTransit;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders.Handlers;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
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
        }
        
        return created;
    }
}