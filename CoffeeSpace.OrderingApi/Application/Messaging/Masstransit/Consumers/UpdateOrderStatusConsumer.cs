using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;

internal sealed class UpdateOrderStatusConsumer : IConsumer<UpdateOrderStatus>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<UpdateOrderStatus> context)
    {
        var order = await _orderRepository.GetByIdAsync(context.Message.OrderId, context.CancellationToken);

        await _orderRepository.UpdateOrderStatusAsync(order!, context.Message.Status, context.CancellationToken);
    }
}