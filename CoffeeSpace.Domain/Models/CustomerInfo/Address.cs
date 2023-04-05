namespace CoffeeSpace.Domain.Models.CustomerInfo;

public sealed class Address
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string State { get; init; }

    public required string StateCode { get; init; }

    public required string Country { get; init; }

    public required string CountryCode { get; init; }

    public required string ZipCode { get; init; }
}