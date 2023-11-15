namespace CoffeeSpace.Client.Models.Ordering;

public sealed class Address
{
    public required string Id { get; init; }

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}