using CoffeeSpace.Domain.Products;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Queries;

public sealed class GetAllProductsQuery : IQuery<IEnumerable<Product>>
{
    
}