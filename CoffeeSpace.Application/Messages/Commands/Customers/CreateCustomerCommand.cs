using CoffeeSpace.Domain.Models.CustomerInfo;
using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.Customers;

public sealed class CreateCustomerCommand : ICommand<bool>
{
    public required Customer Customer { get; init; }
}