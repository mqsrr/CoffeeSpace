using CoffeeSpace.Messages.Products.Events;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using MassTransit;

namespace CoffeeSpace.ProductApi.Application.Messages.Consumers;

internal sealed class AwaitProductsValidationConsumer : IConsumer<AwaitProductsValidation>
{
    private readonly IProductRepository _productRepository;

    public AwaitProductsValidationConsumer(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Consume(ConsumeContext<AwaitProductsValidation> context)
    {
        var products = await _productRepository.GetAllProductsAsync(context.CancellationToken);
        var isValid = context.Message
            .Products
            .All(x => products.Any(product => product.Title == x.Title));
        
        Thread.Sleep(TimeSpan.FromSeconds(4));
        if (!isValid)
        {
            await context.RespondAsync<Fault<AwaitProductsValidation>>(context.Message);
            return;
        }
        
        await context.RespondAsync<OrderStockValidationResult>(new
        {
            context.Message.Order,
            IsValid = isValid
        });
    }
}