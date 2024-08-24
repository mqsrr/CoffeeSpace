using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;

namespace CoffeeSpace.OrderingApi.Application.Contracts.Responses.Buyers;

public sealed class BuyerResponse
{
    public required Guid Id { get; init; }  
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }

    public IEnumerable<OrderResponse>? Orders { get; init; }
}