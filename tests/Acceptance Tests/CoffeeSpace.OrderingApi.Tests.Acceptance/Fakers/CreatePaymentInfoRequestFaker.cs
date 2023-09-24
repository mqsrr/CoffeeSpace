using AutoBogus;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Fakers;

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