using CoffeeSpace.Messages.Products.Events;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using MassTransit;

namespace CoffeeSpace.ProductApi.Application.Messages.Consumers;

internal sealed class OrderStockValidationConsumer : IConsumer<OrderStockValidation>
{
    private readonly IProductRepository _productRepository;

    public OrderStockValidationConsumer(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Consume(ConsumeContext<OrderStockValidation> context)
    {
        var products = await _productRepository.GetAllProductsAsync(context.CancellationToken);
        var isValid = context.Message
            .Products
            .All(x => products.Any(product => product.Title == x.Title));
        
        Thread.Sleep(TimeSpan.FromSeconds(5));
        if (!isValid)
        {
            await context.RespondAsync<Fault<OrderStockValidation>>(context.Message);
            return;
        }
        
        await context.RespondAsync<OrderStockValidationResult>(new
        {
            context.Message.Order,
            IsValid = isValid
        });
    }
}