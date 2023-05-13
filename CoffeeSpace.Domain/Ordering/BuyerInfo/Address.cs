namespace CoffeeSpace.Domain.Ordering.BuyerInfo;

public sealed class Address
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}