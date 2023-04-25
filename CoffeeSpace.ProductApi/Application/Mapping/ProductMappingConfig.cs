using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.ProductApi.Application.Mapping;

[Mapper(EnabledConversions = MappingConversionType.All)]
internal static partial class ProductMappingConfig
{
    public static partial ProductResponse ToResponse(this Product product);
    
    public static partial Product ToProduct(this CreateProductRequest request);
 
    public static partial Product ToProduct(this UpdateProductRequest request);
}