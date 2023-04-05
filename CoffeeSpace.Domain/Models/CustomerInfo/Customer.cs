using CoffeeSpace.Domain.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Domain.Models.Orders;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.Domain.Models.CustomerInfo;

public sealed class Customer : IdentityUser
{
    override public required string Id { get; set; } = Guid.NewGuid().ToString();
    
    public required string Name { get; init; }
    
    public required string PictureUrl { get; init; } = "default";
    
    public required string LastName { get; init; }
    
    override public required string? Email { get; set; }
    
    public required string Password { get; init; }
    
    public required DateTime Birthday { get; init; }
    
    public required string PaymentId { get; init; }
    
    public required PaymentInfo PaymentInfo { get; init; }
    
    public required string AddressId { get; init; }
    
    public required Address Address { get; init; }

    public ICollection<Order> Orders { get; init; } = default!;
}