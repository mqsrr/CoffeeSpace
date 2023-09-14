namespace CoffeeSpace.Client.Contracts.Ordering;

public sealed class CreateAddressRequest
{
    public string Street { get; init; }

    public string City { get; init; }

    public string Country { get; init; }
}