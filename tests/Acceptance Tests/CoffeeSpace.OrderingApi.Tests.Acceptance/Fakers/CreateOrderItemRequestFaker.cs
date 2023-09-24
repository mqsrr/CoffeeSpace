using AutoBogus;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Fakers;

internal sealed class CreateOrderItemRequestFaker : AutoFaker<CreateOrderItemRequest>
{
    public CreateOrderItemRequestFaker()
    {
        RuleFor(item => item.Title, faker => faker.Commerce.ProductName());
        RuleFor(item => item.Description, faker => faker.Commerce.ProductDescription());
        RuleFor(item => item.Quantity, faker => faker.Random.Int(1, 5));
        RuleFor(item => item.Discount, faker => faker.Random.Float());
        RuleFor(item => item.UnitPrice, faker => faker.Random.Float(1, 5));
    }
}