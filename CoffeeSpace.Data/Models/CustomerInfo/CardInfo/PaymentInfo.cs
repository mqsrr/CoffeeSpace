namespace CoffeeSpace.Data.Models.CustomerInfo.CardInfo;

public sealed class PaymentInfo
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CardNumber { get; set; } = null!;
    public string SecurityNumber { get; set; } = null!;
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public CardType CardType { get; set; }

}
