using CoffeeSpace.Client.Contracts.Products;
using CoffeeSpace.Client.WebApiClients;
using Mediator;

namespace CoffeeSpace.Client.Messages.Queries.Handlers;

public sealed class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    private readonly IProductsWebApi _productsWebApi;

    public GetAllProductsQueryHandler(IProductsWebApi productsWebApi)
    {
        _productsWebApi = productsWebApi;
    }

    public async ValueTask<IEnumerable<ProductResponse>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _productsWebApi.GetAllProductsAsync(cancellationToken);
        
        return products;
    }
}