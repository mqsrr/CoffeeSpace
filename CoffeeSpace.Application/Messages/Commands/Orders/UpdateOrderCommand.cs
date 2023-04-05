using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.Orders;

public class UpdateOrderCommand : ICommand<Order>
{
    public required Order Order { get; init; }
}