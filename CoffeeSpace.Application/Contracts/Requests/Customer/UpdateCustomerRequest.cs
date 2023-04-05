using CoffeeSpace.Domain.Models.CustomerInfo;
using CoffeeSpace.Domain.Models.CustomerInfo.CardInfo;

namespace CoffeeSpace.Application.Contracts.Requests.Customer;

public sealed class UpdateCustomerRequest
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string LastName { get; init; }

    public required string PictureUrl { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }

    public required DateOnly Birthday { get; init; }

    public required PaymentInfo PaymentInfo { get; init; }

    public required Address Address { get; init; }
}