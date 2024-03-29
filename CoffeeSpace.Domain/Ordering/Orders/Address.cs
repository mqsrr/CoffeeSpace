namespace CoffeeSpace.Domain.Ordering.Orders;

public sealed class Address
{
    public required Guid Id { get; init; }

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}