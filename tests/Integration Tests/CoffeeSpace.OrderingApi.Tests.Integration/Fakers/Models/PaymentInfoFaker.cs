using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;

public sealed class PaymentInfoFaker : AutoFaker<PaymentInfo>
{
    public PaymentInfoFaker()
    {
        RuleFor(order => order.Id, faker => faker.Random.Guid().ToString());
        UseSeed(69);

        RuleFor(order => order.CardNumber, faker => faker.Finance.CreditCardNumber());
        RuleFor(order => order.SecurityNumber, faker => faker.Finance.CreditCardCvv());
        RuleFor(order => order.ExpirationMonth, faker => faker.Random.Number(1, 12));
        RuleFor(order => order.ExpirationYear, faker => faker.Date.Future(10).Year);
    }
}