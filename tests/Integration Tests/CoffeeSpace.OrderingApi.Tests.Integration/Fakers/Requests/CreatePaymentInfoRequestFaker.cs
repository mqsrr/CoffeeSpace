using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.PaymentInfo;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;

internal sealed class CreatePaymentInfoRequestFaker : AutoFaker<CreatePaymentInfoRequest>
{
    public CreatePaymentInfoRequestFaker()
    {
        RuleFor(request => request.CardNumber, faker => faker.Finance.CreditCardNumber());
        RuleFor(request => request.CardType, CardType.Visa);
        RuleFor(request => request.SecurityNumber, faker => faker.Finance.CreditCardCvv());
        RuleFor(request => request.ExpirationMonth, faker => faker.Random.Int(1, 12));
        RuleFor(request => request.ExpirationYear, 2045);
    }
}