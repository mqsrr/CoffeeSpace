namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

public sealed class DeleteBuyerByIdRequest
{
    public required Guid Id { get; init; }
}