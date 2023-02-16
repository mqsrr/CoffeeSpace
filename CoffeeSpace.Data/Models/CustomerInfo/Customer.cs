using System.ComponentModel.DataAnnotations.Schema;
using CoffeeSpace.Data.Models.CustomerInfo.CardInfo;
using CoffeeSpace.Data.Models.Orders;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.Data.Models.CustomerInfo;

public sealed class Customer : IdentityUser
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string PictureUrl { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public override string Email { get; set; } = null!;
    public string Password { get; init; } = null!;
    public DateOnly Birthday { get; set; }
    
    
    public string PaymentId { get; set; } = null!;
    public PaymentInfo PaymentInfo { get; set; } = null!;
    public string AddressId { get; set; } = null!;
    public Address Address { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = null!;
}