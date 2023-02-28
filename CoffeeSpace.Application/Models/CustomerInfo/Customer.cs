using CoffeeSpace.Application.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Application.Models.Orders;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.Application.Models.CustomerInfo;

public sealed class Customer : IdentityUser
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; init; }
    public required string PictureUrl { get; set; } = "default";
    public required string LastName { get; init; }
    public override required string? Email { get; set; }
    public required string Password { get; set; }
    public required DateOnly Birthday { get; init; }
    
    public required string PaymentId { get; set; }
    public required PaymentInfo PaymentInfo { get; set; }
    public required string AddressId { get; set; }
    public required Address Address { get; set; }

    public ICollection<Order> Orders { get; init; }
}