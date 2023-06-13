namespace CoffeeSpace.Domain.Ordering.BuyerInfo;

public sealed class Address
{
    public required string Id { get; init; }

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}