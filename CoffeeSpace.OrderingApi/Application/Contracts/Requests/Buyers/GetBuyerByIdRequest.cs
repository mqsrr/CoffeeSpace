namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

public sealed class GetBuyerByIdRequest
{
    public required Guid Id { get; init; }
}