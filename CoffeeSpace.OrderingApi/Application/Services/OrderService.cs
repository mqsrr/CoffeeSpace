using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Orders;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Services;

internal sealed class OrderService : IOrderService
{
    private readonly ISender _sender;

    public OrderService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IEnumerable<Order>> GetAllByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken)
    {
        var orders = await _sender.Send(new GetAllOrdersByBuyerIdQuery
        {
            BuyerId = buyerId.ToString()
        }, cancellationToken);

        return orders;
    }

    public async Task<Order?> GetByIdAsync(Guid id, Guid buyerId, CancellationToken cancellationToken)
    {

        var order = await _sender.Send(new GetOrderByIdQuery
        {
            BuyerId = buyerId.ToString(),
            Id = id.ToString()
        }, cancellationToken);

        return order;
    }

    public async Task<bool> CreateAsync(Order order, CancellationToken cancellationToken)
    {
        var created = await _sender.Send(new CreateOrderCommand
        {
            Order = order
        }, cancellationToken);

        return created;
    }

    public async Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new UpdateOrderCommand
        {
            Order = order
        }, cancellationToken);

        return result;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, Guid buyerId, CancellationToken cancellationToken)
    {
        var deleted = await _sender.Send(new DeleteOrderByIdCommand
        {
            Id = id.ToString(),
            BuyerId = buyerId.ToString()
        }, cancellationToken);

        return deleted;
    }
}