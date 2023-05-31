namespace CoffeeSpace.Client.Contracts.Ordering;

public sealed class BuyerResponse
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string Email { get; init; }

    public IEnumerable<OrderResponse> Orders { get; init; }
}