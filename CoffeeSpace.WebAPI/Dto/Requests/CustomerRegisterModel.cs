using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.CustomerInfo.CardInfo;

namespace CoffeeSpace.WebAPI.Dto.Requests;

public sealed class CustomerRegisterModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public Address Address { get; set; } = default!;
    public PaymentInfo PaymentInfo { get; set; } = default!;
}