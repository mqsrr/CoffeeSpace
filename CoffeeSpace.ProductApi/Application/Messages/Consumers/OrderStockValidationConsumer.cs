using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using MassTransit;

namespace CoffeeSpace.ProductApi.Application.Messages.Consumers;

internal sealed class OrderStockValidationConsumer : IConsumer<ValidateOrderStock>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<OrderStockValidationConsumer> _logger;

    public OrderStockValidationConsumer(IProductRepository productRepository, ILogger<OrderStockValidationConsumer> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ValidateOrderStock> context)
    {
        var existingProducts = await _productRepository.GetAllProductsAsync(context.CancellationToken);
        var existingTitles = existingProducts.Select(product => product.Title);
        bool isValid = context.Message.ProductTitles.All(title => existingTitles.Contains(title));
        
        if (!isValid)
        {
            _logger.LogInformation("The order with ID {OrderId} has invalid products, which are no longer acceptable or out of stock", context.Message.Order.Id);
            await context.RespondAsync<Fault<ValidateOrderStock>>(context.Message);
            return;
        }
        
        _logger.LogInformation("The order with ID {OrderId} has successfully completed product validation", context.Message.Order.Id);
        await context.RespondAsync<OrderStockConfirmed>(new
        {
            context.Message.Order,
            IsValid = isValid
        });
    }
}