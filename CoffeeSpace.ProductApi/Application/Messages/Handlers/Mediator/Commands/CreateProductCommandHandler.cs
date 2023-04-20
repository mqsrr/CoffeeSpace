using CoffeeSpace.ProductApi.Application.Messages.Commands;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Handlers.Mediator.Commands;

internal sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<bool> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var created = await _productRepository.CreateProductAsync(command.Product, cancellationToken);

        return created;
    }
}