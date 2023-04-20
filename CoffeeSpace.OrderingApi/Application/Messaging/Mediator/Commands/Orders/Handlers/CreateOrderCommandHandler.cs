using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders.Handlers;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async ValueTask<bool> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var created = await _orderRepository.CreateAsync(command.Order, cancellationToken);

        return created;
    }
}