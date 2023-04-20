using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Messages.Queries;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Handlers.Mediator.Queries;

internal sealed class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<IEnumerable<Product>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllProductsAsync(cancellationToken);

        return products;
    }
}