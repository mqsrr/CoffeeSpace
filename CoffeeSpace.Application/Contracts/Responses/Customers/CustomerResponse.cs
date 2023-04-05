using CoffeeSpace.Domain.Models.CustomerInfo;
using CoffeeSpace.Domain.Models.CustomerInfo.CardInfo;

namespace CoffeeSpace.Application.Contracts.Responses.Customers;

public sealed class CustomerResponse
{
    public required string Name { get; init; }

    public required string LastName { get; init; }

    public required string Email { get; init; }

    public required DateTime Birthday { get; init; }

    public required string Password { get; init; }

    public required string PictureUrl { get; init; }

    public required PaymentInfo PaymentInfo { get; init; }

    public required Address Address { get; init; }
}