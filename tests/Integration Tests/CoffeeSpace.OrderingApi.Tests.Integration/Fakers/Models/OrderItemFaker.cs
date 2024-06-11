using AutoBogus;
using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;

public sealed class OrderItemFaker : AutoFaker<OrderItem>
{
    public OrderItemFaker()
    {
        RuleFor(order => order.Id, faker => faker.Random.Guid());
        RuleFor(order => order.Title, faker => faker.Commerce.ProductName());
        RuleFor(order => order.Description, faker => faker.Commerce.ProductAdjective());
        RuleFor(order => order.UnitPrice, faker => faker.Random.Float(1, 5));
        RuleFor(order => order.Discount, faker => faker.Random.Float());
        RuleFor(order => order.Quantity, faker => faker.Random.Number(1, 9));
    }
}