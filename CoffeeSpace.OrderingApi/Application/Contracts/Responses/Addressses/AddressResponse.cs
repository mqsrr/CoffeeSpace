namespace CoffeeSpace.OrderingApi.Application.Contracts.Responses.Addressses;

public sealed class AddressResponse
{
    public required Guid Id { get; init; }

    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }
}