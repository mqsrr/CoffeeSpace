using AutoBogus;
using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;

public sealed class OrderFaker : AutoFaker<Order>
{
    public OrderFaker(Guid buyerId)
    {
        RuleFor(order => order.Id, faker => faker.Random.Guid());

        RuleFor(order => order.BuyerId, buyerId);
        RuleFor(order => order.Address, () => AutoFaker.Generate<Address, AddressFaker>());
        RuleFor(order => order.OrderItems, () =>  AutoFaker.Generate<OrderItem, OrderItemFaker>(3));
    }
}