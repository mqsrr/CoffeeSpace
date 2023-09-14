using CoffeeSpace.Client.Models.Products;
using CoffeeSpace.Client.WebApiClients;
using Mediator;

namespace CoffeeSpace.Client.Messages.Queries.Handlers;

public sealed class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IProductsWebApi _productsWebApi;

    public GetAllProductsQueryHandler(IProductsWebApi productsWebApi)
    {
        _productsWebApi = productsWebApi;
    }

    public async ValueTask<IEnumerable<Product>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _productsWebApi.GetAllProductsAsync(cancellationToken);
        return products.Items;
    }
}