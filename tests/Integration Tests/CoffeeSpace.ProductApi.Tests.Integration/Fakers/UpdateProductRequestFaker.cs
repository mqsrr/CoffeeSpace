using AutoBogus;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

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
        RuleFor(product => product.UnitPrice, faker => faker.Random.Float(1, 10));
        RuleFor(product => product.Image, faker =>
        {
            var image = Image.Load("..//..//..//1.jpg"); 
            var memoryStream = new MemoryStream();
            image.SaveAsJpeg(memoryStream);

            return new FormFile(memoryStream, 0, memoryStream.Length, "Image", "1.jpg");
        });
    }
}