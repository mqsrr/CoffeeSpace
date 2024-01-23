using AutoBogus;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;

namespace CoffeeSpace.ProductApi.Tests.Integration.Fakers;

public sealed class CreateProductRequestFaker : AutoFaker<CreateProductRequest>
{
    public CreateProductRequestFaker()
    {
        UseSeed(69);
        
        RuleFor(product => product.Title, faker => faker.Commerce.ProductName());
        RuleFor(product => product.Description, faker => faker.Commerce.ProductDescription());
        RuleFor(product => product.Quantity, faker => faker.Random.Int(0, 99));
        RuleFor(product => product.UnitPrice, faker => faker.Random.Float(1, 10));
    }
}