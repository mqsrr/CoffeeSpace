using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.Orders;

public sealed class CreateOrderCommand : ICommand<bool>
{
    public required Order Order { get; init; }
}