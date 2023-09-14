
using CoffeeSpace.Client.Models.Ordering;

namespace CoffeeSpace.Client.Contracts.Ordering;

public sealed class CreatePaymentInfoRequest
{
    public string CardNumber { get; init; }

    public string SecurityNumber { get; init; }

    public int ExpirationMonth { get; init; }

    public int ExpirationYear { get; init; }

    public int CardType { get; init; }
}