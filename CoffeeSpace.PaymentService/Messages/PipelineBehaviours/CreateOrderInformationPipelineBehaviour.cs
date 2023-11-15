using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.PaymentService.Mappers;
using CoffeeSpace.PaymentService.Messages.Commands;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using Mediator;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Messages.PipelineBehaviours;

internal sealed class CreateOrderInformationPipelineBehaviour : IPipelineBehavior<CreatePaypalOrderCommand, Order?>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICacheService<Order> _cacheService;

    public CreateOrderInformationPipelineBehaviour(IPaymentRepository paymentRepository, ICacheService<Order> cacheService)
    {
        _paymentRepository = paymentRepository;
        _cacheService = cacheService;
    }

    public async ValueTask<Order?> Handle(CreatePaypalOrderCommand message, CancellationToken cancellationToken, MessageHandlerDelegate<CreatePaypalOrderCommand, Order?> next)
    {
        var createdPaypalOrder = await next(message, cancellationToken);
        if (createdPaypalOrder is null)
        {
            return null;
        }

        var applicationOrder = message.ApplicationOrder;
        var paypalOrderInformation = createdPaypalOrder.ToPaypalOrderInformation(applicationOrder);

        await _cacheService.SetAsync($"payment-{createdPaypalOrder.Id}", JsonConvert.SerializeObject(applicationOrder), cancellationToken);
        await _paymentRepository.CreatePaymentAsync(paypalOrderInformation, cancellationToken);

        return createdPaypalOrder;
    }
}