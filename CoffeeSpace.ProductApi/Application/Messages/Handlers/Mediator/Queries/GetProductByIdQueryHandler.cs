using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Messages.Queries;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Handlers.Mediator.Queries;

internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Product?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(query.Id, cancellationToken);

        return product;
    }
}