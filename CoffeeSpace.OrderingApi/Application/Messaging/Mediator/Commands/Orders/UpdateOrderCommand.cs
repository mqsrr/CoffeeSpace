using CoffeeSpace.Domain.Ordering.Orders;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders;

public sealed class UpdateOrderCommand : ICommand<Order?>
{
    public required Order Order { get; init; }
}