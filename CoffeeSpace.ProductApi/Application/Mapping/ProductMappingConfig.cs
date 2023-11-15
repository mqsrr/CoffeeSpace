using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.ProductApi.Application.Mapping;

[Mapper]
internal static partial class ProductMappingConfig
{
    public static partial ProductResponse ToResponse(this Product product);
    
    public static partial Product ToProduct(this CreateProductRequest request);

   private static partial Product ToProduct(this UpdateProductRequest request);

    public static Product ToProduct(this UpdateProductRequest request, Guid id)
    {
        request.Id = id;

        return request.ToProduct();
    }
}