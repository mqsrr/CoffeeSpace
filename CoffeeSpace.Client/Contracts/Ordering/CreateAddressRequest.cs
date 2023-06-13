namespace CoffeeSpace.Client.Contracts.Ordering;

public sealed class CreateAddressRequest
{
    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}