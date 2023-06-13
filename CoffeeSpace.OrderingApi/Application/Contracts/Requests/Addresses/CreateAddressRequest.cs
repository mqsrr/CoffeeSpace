namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;

public sealed class CreateAddressRequest
{
    public Guid Id { get; } = Guid.NewGuid();

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}