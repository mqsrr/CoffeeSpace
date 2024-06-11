using AutoBogus;
using CoffeeSpace.Domain.Products;
using SixLabors.ImageSharp;

namespace CoffeeSpace.ProductApi.Tests.Integration.Fakers;

public sealed class ProductFaker : AutoFaker<Product>
{
    public ProductFaker()
    {
        RuleFor(product => product.Id, faker => faker.Random.Guid());
        RuleFor(product => product.Title, faker => faker.Commerce.ProductName());
        RuleFor(product => product.Description, faker => faker.Commerce.ProductDescription());
        RuleFor(product => product.Quantity, faker => faker.Random.Int(0, 99));
        RuleFor(product => product.UnitPrice, faker => faker.Random.Float(0, 10));
        RuleFor(product => product.Image, _ =>
        {
            var image = Image.Load("..//..//..//1.jpg"); 
            var memoryStream = new MemoryStream();
            image.SaveAsJpeg(memoryStream);

            return memoryStream.ToArray();
        });
    }
}