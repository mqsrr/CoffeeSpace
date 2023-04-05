using CoffeeSpace.Domain.Models.CustomerInfo;
using Mediator;

namespace CoffeeSpace.Application.Messages.Queries.Customers;

public sealed class GetCustomerByEmailQuery : IQuery<Customer?>
{
    public required string Email { get; init; }
}