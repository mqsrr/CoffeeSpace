namespace CoffeeSpace.ProductApi.Application.Contracts.Requests;

public sealed class GetProductByIdRequest
{
    public required Guid Id { get; init; }
}