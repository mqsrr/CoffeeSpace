using CoffeeSpace.Domain.Products;

namespace CoffeeSpace.ProductApi.Application.Contracts.Responses;

public sealed class ProductResponse
{
    public required string Id { get; init; }

    public required ImageInformation Image { get; init; }    
    
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }
    
    public  required int Quantity { get; init; }
}