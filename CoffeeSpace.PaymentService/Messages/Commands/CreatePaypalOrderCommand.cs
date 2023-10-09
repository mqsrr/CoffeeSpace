using CoffeeSpace.Domain.Ordering.Orders;
using Mediator;
namespace CoffeeSpace.PaymentService.Messages.Commands;

public sealed class CreatePaypalOrderCommand : ICommand<PayPalCheckoutSdk.Orders.Order?>
{
    public required Order ApplicationOrder { get; init; }
}