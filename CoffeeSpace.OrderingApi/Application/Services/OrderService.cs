using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Services;

internal sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderService(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
    }

    public Task<IEnumerable<Order>> GetAllByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken)
    {
        var orders = _orderRepository.GetAllByBuyerIdAsync(buyerId.ToString(), cancellationToken);
        return orders;
    }

    public Task<Order?> GetByIdAsync(Guid id, Guid buyerId, CancellationToken cancellationToken)
    {
        var order = _orderRepository.GetByIdAsync(id.ToString(), cancellationToken);
        return order;
    }

    public async Task<bool> CreateAsync(Order order, CancellationToken cancellationToken)
    {
        bool isCreated = await _orderRepository.CreateAsync(order, cancellationToken);
        if (isCreated)
        {
            await _publishEndpoint.Publish<SubmitOrder>(new
            {
                Order = order
            }, cancellationToken).ConfigureAwait(false);
        }

        return isCreated;
    }
    
    public Task<bool> DeleteByIdAsync(Guid id, Guid buyerId, CancellationToken cancellationToken)
    {
        var isDeleted = _orderRepository.DeleteByIdAsync(id.ToString(), cancellationToken);
        return isDeleted;
    }
}