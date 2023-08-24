using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo;
using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;

public sealed class OrderFaker : AutoFaker<Order>
{
    public OrderFaker(string buyerId, Address address, PaymentInfo paymentInfo, IEnumerable<OrderItem> orderItems)
    {
        RuleFor(order => order.Id, faker => faker.Random.Guid().ToString());
        UseSeed(69);

        RuleFor(order => order.BuyerId, buyerId);
        RuleFor(order => order.Address, address);
        RuleFor(order => order.PaymentInfo, paymentInfo);
        RuleFor(order => order.OrderItems, orderItems.ToList());
    }
}