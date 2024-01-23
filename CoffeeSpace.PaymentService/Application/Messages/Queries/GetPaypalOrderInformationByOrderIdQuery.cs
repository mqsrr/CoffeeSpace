using CoffeeSpace.PaymentService.Application.Models;
using Mediator;

namespace CoffeeSpace.PaymentService.Application.Messages.Queries;

public sealed class GetPaypalOrderInformationByOrderIdQuery : IQuery<PaypalOrderInformation>
{
    public required string Id { get; init; }   
}