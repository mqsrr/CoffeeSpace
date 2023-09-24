namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

public sealed class UpdateBuyerRequest
{
    public required string Name { get; init; }
    
    public required string Email { get; init; }
}