using CoffeeSpace.Domain.Models.CustomerInfo;
using Mediator;

namespace CoffeeSpace.Application.Messages.Queries.Customers;

public sealed class GetCustomerByIdQuery : IQuery<Customer?>
{
    public required string Id { get; init; }
}