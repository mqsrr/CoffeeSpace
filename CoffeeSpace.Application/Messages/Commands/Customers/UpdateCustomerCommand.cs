using CoffeeSpace.Domain.Models.CustomerInfo;
using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.Customers;

public sealed class UpdateCustomerCommand : ICommand<Customer?>
{
    public required Customer Customer { get; init; }
}