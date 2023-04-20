namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;

public sealed class UpdateBuyerRequest
{
    public Guid Id { get; set; }
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }
}