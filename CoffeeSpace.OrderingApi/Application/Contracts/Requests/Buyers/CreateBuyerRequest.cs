namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

public sealed class CreateBuyerRequest
{
    public required string Name { get; init; }
    
    public required string Email { get; init; }
}