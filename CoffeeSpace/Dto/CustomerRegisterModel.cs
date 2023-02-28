using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Models.CustomerInfo.CardInfo;

namespace CoffeeSpace.Dto;

public sealed class CustomerRegisterModel
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Birthday { get; set; }
    public string Password { get; set; }

    public Address Address { get; set; }
    public PaymentInfo PaymentInfo { get; set; }
}