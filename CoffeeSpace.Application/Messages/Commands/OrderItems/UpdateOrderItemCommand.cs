using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.OrderItems;

public sealed class UpdateOrderItemCommand : ICommand<OrderItem?>
{
    public required OrderItem OrderItem { get; init; }
}