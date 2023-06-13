using CoffeeSpace.Messages.Ordering.Events;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using MassTransit;

namespace CoffeeSpace.PaymentService.Consumers;

internal sealed class OrderPaymentValidationConsumer : IConsumer<OrderPaymentValidation>
{
    private readonly IPaymentHistoryRepository _historyRepository;
    private readonly ILogger<OrderPaymentValidationConsumer> _logger;

    public OrderPaymentValidationConsumer(IPaymentHistoryRepository historyRepository, ILogger<OrderPaymentValidationConsumer> logger)
    {
        _historyRepository = historyRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderPaymentValidation> context)
    {
        var order = context.Message.Order;
        var isValid = await _historyRepository.CreateAsync(new PaymentHistory
        {
            Id = Guid.NewGuid().ToString(),
            OrderId = order.Id,
            PaymentId = order.PaymentInfo.Id,
            OrderDate = DateTime.UtcNow,
            TotalPrice = order.OrderItems.Sum(x => x.Total)
        });

        await Task.Delay(TimeSpan.FromSeconds(3));
        _logger.LogInformation("The payment for the order with ID {OrderId} has been stored", order.Id);
        
        await context.RespondAsync<OrderPaymentValidationResult>(new
        {
            Order = order,
            IsValid = isValid
        });
    }
}