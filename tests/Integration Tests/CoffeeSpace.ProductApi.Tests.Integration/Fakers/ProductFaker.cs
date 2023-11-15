using AutoBogus;
using CoffeeSpace.Domain.Products;

namespace CoffeeSpace.ProductApi.Tests.Integration.Fakers;

public sealed class ProductFaker : AutoFaker<Product>
{
    public ProductFaker()
    {
        UseSeed(69);
        
        RuleFor(product => product.Id, faker => faker.Random.Guid().ToString());
        RuleFor(product => product.Title, faker => faker.Commerce.ProductName());
        RuleFor(product => product.Description, faker => faker.Commerce.ProductDescription());
        RuleFor(product => product.Quantity, faker => faker.Random.Int(0, 99));
        RuleFor(product => product.Discount, faker => faker.Random.Float());
        RuleFor(product => product.UnitPrice, faker => faker.Random.Float(0, 10));
    }
}