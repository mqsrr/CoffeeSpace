using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Models.CustomerInfo.CardInfo;

namespace CoffeeSpace.Contracts.Requests.Customer;

public sealed record CreateCustomerRequest(
    string Name,
    string LastName,
    string PictureUrl,
    string Email,
    string Password,
    DateOnly Birthday,
    PaymentInfo PaymentInfo,
    Address Address);