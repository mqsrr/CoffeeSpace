namespace CoffeeSpace.AClient.Contracts.Requests.Orders;

public sealed class CreateAddressRequest
{
    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;
}