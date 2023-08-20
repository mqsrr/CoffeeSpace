using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders.Handlers;

internal sealed class DeleteOrderByIdCommandHandler : ICommandHandler<DeleteOrderByIdCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderByIdCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async ValueTask<bool> Handle(DeleteOrderByIdCommand command, CancellationToken cancellationToken)
    {
        bool deleted = await _orderRepository.DeleteByIdAsync(command.Id, cancellationToken);

        return deleted;
    }
}