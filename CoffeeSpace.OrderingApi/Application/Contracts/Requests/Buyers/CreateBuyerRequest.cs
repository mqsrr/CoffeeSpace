namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

public sealed class CreateBuyerRequest
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
}