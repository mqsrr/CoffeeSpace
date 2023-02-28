namespace CoffeeSpace.Application.Models.CustomerInfo.CardInfo;

public sealed class PaymentInfo
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public required string CardNumber { get; init; }
    public required string SecurityNumber { get; init; }
    public required int ExpirationMonth { get; init; }
    public required int ExpirationYear { get; init; }
    public CardType CardType { get; init; }

}
