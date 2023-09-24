namespace CoffeeSpace.ProductApi.Application.Contracts.Requests;

public sealed class GetPagedProductsRequest
{
    public required int Page { get; init; }

    public required int PageSize { get; init; }
}