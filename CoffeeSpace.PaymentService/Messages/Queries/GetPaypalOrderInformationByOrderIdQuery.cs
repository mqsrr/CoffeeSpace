using CoffeeSpace.PaymentService.Models;
using Mediator;

namespace CoffeeSpace.PaymentService.Messages.Queries;

public sealed class GetPaypalOrderInformationByOrderIdQuery : IQuery<PaypalOrderInformation>
{
    public required string Id { get; init; }   
}