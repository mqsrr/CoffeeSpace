using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using CoffeeSpace.PaymentService.Domain;
using MassTransit;

namespace CoffeeSpace.PaymentService.Application.Consumers;

internal sealed class OrderPaymentValidationConsumer : IConsumer<OrderPaymentValidation>
{
    private readonly IPaymentHistoryRepository _historyRepository;

    public OrderPaymentValidationConsumer(IPaymentHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public async Task Consume(ConsumeContext<OrderPaymentValidation> context)
    {
        //Payment Operations

        var order = context.Message.Order;
        var isValid = await _historyRepository.CreateAsync(new PaymentHistory
        {
            Id = Guid.NewGuid().ToString(),
            OrderId = order.Id,
            PaymentId = order.PaymentInfoId,
            OrderDate = DateTime.UtcNow,
            TotalPrice = order.OrderItems.Sum(x => x.Total)
        });
        Thread.Sleep(TimeSpan.FromSeconds(7));

        await context.RespondAsync<OrderPaymentValidationResult>(new
        {
            Order = order,
            IsValid = isValid
        });
    }
}