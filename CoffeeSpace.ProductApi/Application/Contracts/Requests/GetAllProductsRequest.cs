namespace CoffeeSpace.ProductApi.Application.Contracts.Requests;

public sealed class GetAllProductsRequest
{
    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 5;
}