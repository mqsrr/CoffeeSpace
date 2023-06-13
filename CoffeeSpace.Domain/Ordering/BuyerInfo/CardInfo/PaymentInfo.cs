namespace CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo;

public sealed class PaymentInfo
{
    public required string Id { get; init; }

    public required string CardNumber { get; init; }

    public required string SecurityNumber { get; init; }

    public required int ExpirationMonth { get; init; }

    public required int ExpirationYear { get; init; }

    public required CardType CardType { get; init; }
}