namespace CoffeeSpace.AClient.Contracts.Requests.Orders;

public sealed class CreateAddressRequest
{
    public string Street { get; init; } = null!;

    public string City { get; init; } = null!;

    public string Country { get; init; } = null!;
}