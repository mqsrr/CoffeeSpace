namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

public sealed class BuyerResponse
{
    public required string Id { get; init; }  
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }

    public IEnumerable<OrderResponse>? Orders { get; init; } = Array.Empty<OrderResponse>();
}