namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

public sealed class GetBuyerByEmailRequest
{
    public required string Email { get; init; }
}