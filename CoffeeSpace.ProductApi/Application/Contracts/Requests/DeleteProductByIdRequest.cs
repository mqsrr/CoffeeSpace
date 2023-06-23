namespace CoffeeSpace.ProductApi.Application.Contracts.Requests;

public sealed class DeleteProductByIdRequest
{
    public required Guid Id { get; init; }
}