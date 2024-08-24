using System;
using System.IO;
using AutoBogus;
using CoffeeSpace.AClient.Models;
using SixLabors.ImageSharp;

namespace CoffeeSpace.AClient.Fakers;

internal sealed class CoffeeFaker : AutoFaker<Product>
{
    public CoffeeFaker()
    {
        RuleFor(product => product.Image, faker =>
        {
            var image = Image.Load("D:\\Codes\\CoffeeSpace\\CoffeeSpace.AClient\\Assets\\americano.jpeg");
            var memoryStream = new MemoryStream();
            
            image.SaveAsJpeg(memoryStream);
            return new ImageInformation
            {
                Mime = image.Metadata.DecodedImageFormat!.DefaultMimeType,
                Data = memoryStream.ToArray().ToString()!
            };
        });
        RuleFor(product => product.Title, faker => faker.Commerce.ProductName());
        RuleFor(product => product.Description, faker => faker.Commerce.ProductDescription());
        RuleFor(product => product.UnitPrice, faker => MathF.Round(faker.Random.Float(1f, 4f)));
        RuleFor(product => product.Quantity, _ => 1);
    }
}