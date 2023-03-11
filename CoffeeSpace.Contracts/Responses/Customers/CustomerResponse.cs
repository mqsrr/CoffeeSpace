using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Models.CustomerInfo.CardInfo;

namespace CoffeeSpace.Contracts.Responses.Customers;

public sealed record CustomerResponse(
    string Name,
    string LastName,
    string Email,
    PaymentInfo PaymentInfo,
    Address Address);