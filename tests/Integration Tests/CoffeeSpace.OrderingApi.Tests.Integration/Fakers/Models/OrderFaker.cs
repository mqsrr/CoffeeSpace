using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;

public sealed class OrderFaker : AutoFaker<Order>
{
    public OrderFaker(Guid buyerId, Address address, IEnumerable<OrderItem> orderItems)
    {
        RuleFor(order => order.Id, faker => faker.Random.Guid());
        UseSeed(69);

        RuleFor(order => order.BuyerId, buyerId);
        RuleFor(order => order.Address, address);
        RuleFor(order => order.OrderItems, orderItems.ToList());
    }
}