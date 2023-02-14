namespace CoffeeSpace.Data.Models.CustomerInfo;

public sealed class Address
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string StateCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string CountryCode { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
}
