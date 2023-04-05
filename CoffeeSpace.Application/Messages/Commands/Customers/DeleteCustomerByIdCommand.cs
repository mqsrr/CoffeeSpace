using Mediator;

namespace CoffeeSpace.Application.Messages.Commands.Customers;

public sealed class DeleteCustomerByIdCommand : ICommand<bool>
{
    public required string Id { get; init; }
}