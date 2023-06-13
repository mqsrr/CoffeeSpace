using CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo;

namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.PaymentInfo;

public sealed class CreatePaymentInfoRequest
{
    public Guid Id { get; } = Guid.NewGuid();

    public required string CardNumber { get; init; }

    public required string SecurityNumber { get; init; }

    public required int ExpirationMonth { get; init; }

    public required int ExpirationYear { get; init; }

    public required CardType CardType { get; init; }
}