namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

public sealed class AddressResponse
{
    public required string Id { get; init; }

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}