using CoffeeSpace.Client.Models.Ordering;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands;

public sealed class ConfirmOrdersStockCommand : ICommand
{
    public required IEnumerable<OrderItem> OrderItems { get; set; }
}
