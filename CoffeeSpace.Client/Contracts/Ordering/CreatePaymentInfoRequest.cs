using CoffeeSpace.Client.Models.Ordering;

namespace CoffeeSpace.Client.Contracts.Ordering;

public sealed class CreatePaymentInfoRequest
{
    public required string CardNumber { get; init; }

    public required string SecurityNumber { get; init; }

    public required int ExpirationMonth { get; init; }

    public required int ExpirationYear { get; init; }

    public required CardType CardType { get; init; }
}