namespace CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;

public sealed class UpdateAddressRequest
{
    public Guid Id { get; internal set; }

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}