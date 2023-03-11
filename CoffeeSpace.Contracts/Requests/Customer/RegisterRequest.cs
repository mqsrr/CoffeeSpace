using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Models.CustomerInfo.CardInfo;

namespace CoffeeSpace.Contracts.Requests.Customer;

public sealed record RegisterRequest(
    string Name,
    string LastName,
    string Username,
    string Email,
    string Password,
    Address Address,
    PaymentInfo PaymentInfo);