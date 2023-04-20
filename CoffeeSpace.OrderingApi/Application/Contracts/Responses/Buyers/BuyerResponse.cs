namespace CoffeeSpace.OrderingApi.Application.Contracts.Responses.Buyers;

public sealed class BuyerResponse
{
    public required string Id { get; init; }  
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
}