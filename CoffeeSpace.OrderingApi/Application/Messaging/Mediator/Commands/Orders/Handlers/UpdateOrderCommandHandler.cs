using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders.Handlers;

internal sealed class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, Order?>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async ValueTask<Order?> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var updatedOrder = await _orderRepository.UpdateAsync(command.Order, cancellationToken);

        return updatedOrder;
    }
}