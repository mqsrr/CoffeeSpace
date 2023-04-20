using CoffeeSpace.ProductApi.Application.Messages.Commands;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Handlers.Mediator.Commands;

internal sealed class DeleteProductByIdCommandHandler : ICommandHandler<DeleteProductByIdCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductByIdCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<bool> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
    {
        var deleted = await _productRepository.DeleteProductByIdAsync(command.Id, cancellationToken);

        return deleted;
    }
}