using CoffeeSpace.Data.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Data.Models.Orders;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.Data.Models.CustomerInfo;

public sealed partial class Customer : IdentityUser
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public string PictureUrl { get; set; } = "default";
    public string LastName { get; set; } = default!;
    public override string Email { get; set; } = default!;
    public string Password { get; init; } = default!;
    public DateOnly Birthday { get; set; }
    
    
    public string PaymentId { get; set; } = default!;
    public PaymentInfo PaymentInfo { get; set; } = default!;
    public string AddressId { get; set; } = default!;
    public Address Address { get; set; } = default!;

    public ICollection<Order> Orders { get; set; } = default!;
}