using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Orders.Handlers;

internal sealed class GetAllOrdersByBuyerIdQueryHandler : IQueryHandler<GetAllOrdersByBuyerIdQuery, IEnumerable<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersByBuyerIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async ValueTask<IEnumerable<Order>> Handle(GetAllOrdersByBuyerIdQuery query, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllByBuyerIdAsync(query.BuyerId, cancellationToken);

        return orders;
    }
}