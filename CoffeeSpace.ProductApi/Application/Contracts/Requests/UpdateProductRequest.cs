namespace CoffeeSpace.ProductApi.Application.Contracts.Requests;

public sealed class UpdateProductRequest
{
    public Guid Id { get; set; }
    
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required float Discount { get; init; }

    public  required int Quantity { get; init; }
}