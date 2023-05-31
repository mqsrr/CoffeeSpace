using CoffeeSpace.Domain.Ordering.Orders;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands;

public sealed class CreateOrderRequest : ICommand<bool>
{
    public required Order Order { get; init; }
}