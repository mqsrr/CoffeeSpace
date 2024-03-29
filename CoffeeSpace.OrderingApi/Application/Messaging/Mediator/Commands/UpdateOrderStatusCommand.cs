using CoffeeSpace.Domain.Ordering.Orders;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands;

public sealed class UpdateOrderStatusCommand : ICommand
{
    public required Guid OrderId { get; init; }

    public required Guid BuyerId { get; init; }

    public required OrderStatus Status { get; init; }
}