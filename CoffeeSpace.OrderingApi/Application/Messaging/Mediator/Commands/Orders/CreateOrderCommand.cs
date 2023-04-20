using CoffeeSpace.Domain.Ordering.Orders;
using Mediator;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders;

public sealed class CreateOrderCommand : ICommand<bool>
{
    public required Order Order { get; init; }
}