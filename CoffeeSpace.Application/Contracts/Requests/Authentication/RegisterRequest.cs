using CoffeeSpace.Domain.Models.CustomerInfo;
using CoffeeSpace.Domain.Models.CustomerInfo.CardInfo;

namespace CoffeeSpace.Application.Contracts.Requests.Authentication;

public sealed class RegisterRequest
{
    public required string Name { get; init; }

    public required string LastName { get; init; }

    public required string Username { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }

    public required Address Address { get; init; }

    public required PaymentInfo PaymentInfo { get; init; }
}