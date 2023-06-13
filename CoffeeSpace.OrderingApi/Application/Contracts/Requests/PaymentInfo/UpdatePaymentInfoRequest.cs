using CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo;

namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.PaymentInfo;

public sealed class UpdatePaymentInfoRequest
{
    public Guid Id { get; internal set; }

    public required string CardNumber { get; init; }

    public required string SecurityNumber { get; init; }

    public required int ExpirationMonth { get; init; }

    public required int ExpirationYear { get; init; }

    public required CardType CardType { get; init; }
}