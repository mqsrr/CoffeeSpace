using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using MassTransit;

namespace CoffeeSpace.ProductApi.Application.Messages.Consumers;

internal sealed class OrderStockValidationConsumer : IConsumer<OrderStockValidation>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<OrderStockValidationConsumer> _logger;

    public OrderStockValidationConsumer(IProductRepository productRepository, ILogger<OrderStockValidationConsumer> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderStockValidation> context)
    {
        var products = await _productRepository.GetAllProductsAsync(context.CancellationToken);
        bool isValid = context.Message.Products.All(x => 
            products.Any(product => product.Title.Equals(x.Title, StringComparison.Ordinal)));
        
        if (!isValid)
        {
            _logger.LogInformation("The order with ID {OrderId} has invalid products, which are no longer acceptable or out of stock", context.Message.Order.Id);
            await context.RespondAsync<Fault<OrderStockValidation>>(context.Message);
            
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