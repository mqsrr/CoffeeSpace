using AutoBogus;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;

namespace CoffeeSpace.ProductApi.Tests.Integration.Fakers;

public sealed class UpdateProductRequestFaker : AutoFaker<UpdateProductRequest>
{
    public UpdateProductRequestFaker()
    {
        UseSeed(69);
        
        Ignore(product => product.Id);
        RuleFor(product => product.Title, faker => faker.Commerce.ProductName());
        RuleFor(product => product.Description, faker => faker.Commerce.ProductDescription());
        RuleFor(product => product.Quantity, faker => faker.Random.Int(0, 99));
        RuleFor(product => product.Discount, faker => faker.Random.Float());
        RuleFor(product => product.UnitPrice, faker => faker.Random.Float(1, 10));
    }
}