using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using Riok.Mapperly.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;

namespace CoffeeSpace.ProductApi.Application.Mapping;

[Mapper]
internal static partial class ProductMapper
{
    public static partial ProductResponse ToResponse(this Product product);
    
    public static partial Product ToProduct(this CreateProductRequest request);

   private static partial Product ToProduct(this UpdateProductRequest request);

    public static Product ToProduct(this UpdateProductRequest request, Guid id)
    {
        request.Id = id;

        return request.ToProduct();
    }
    
    private static byte[] MapImageToByte(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        using var image = Image.Load(file.OpenReadStream());

        image.SaveAsync(memoryStream, image.DetectEncoder(file.FileName));
        return memoryStream.ToArray();
    }
    
    private static ImageInformation MapBytesToImage(byte[] imageData)
    {
        using var image = Image.Load(imageData);
        return new ImageInformation
        {
            Mime = image.Metadata.DecodedImageFormat!.DefaultMimeType,
            Data = image.ToBase64String(image.Metadata.DecodedImageFormat)
        };
    }    
}