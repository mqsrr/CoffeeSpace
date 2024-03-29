using System.Buffers;
using System.Net.Mime;
using AutoBogus;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

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
        RuleFor(product => product.Image, faker =>
        {
            var image = Image.Load("..//..//..//1.jpg"); 
            var memoryStream = new MemoryStream();
            image.SaveAsJpeg(memoryStream);

            return new FormFile(memoryStream, 0, memoryStream.Length, "Image", "1.jpg");
        });
    }
}