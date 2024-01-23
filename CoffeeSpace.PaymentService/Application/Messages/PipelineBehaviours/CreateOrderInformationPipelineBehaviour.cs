using CoffeeSpace.PaymentService.Application.Helpers;
using CoffeeSpace.PaymentService.Application.Mappers;
using CoffeeSpace.PaymentService.Application.Messages.Commands;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using CoffeeSpace.Shared.Services.Abstractions;
using Mediator;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Application.Messages.PipelineBehaviours;

internal sealed class CreateOrderInformationPipelineBehaviour : IPipelineBehavior<CreatePaypalOrderCommand, Order?>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICacheService _cacheService;

    public CreateOrderInformationPipelineBehaviour(IPaymentRepository paymentRepository, ICacheService cacheService)
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

        await _cacheService.HashSetAsync(CacheKeys.Payments.HashKey, CacheKeys.Payments.GetById(createdPaypalOrder.Id), JsonConvert.SerializeObject(applicationOrder));
        await _paymentRepository.CreatePaymentAsync(paypalOrderInformation, cancellationToken);

        return createdPaypalOrder;
    }
}