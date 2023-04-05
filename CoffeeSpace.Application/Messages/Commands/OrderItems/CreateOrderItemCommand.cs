using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.OrderItems;

public sealed class CreateOrderItemCommand : ICommand<bool>
{
    public required OrderItem OrderItem { get; init; }
}