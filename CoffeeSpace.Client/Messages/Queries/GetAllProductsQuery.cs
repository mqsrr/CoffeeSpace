using CoffeeSpace.Client.Contracts.Products;
using Mediator;

namespace CoffeeSpace.Client.Messages.Queries;

public sealed class GetAllProductsQuery : IQuery<IEnumerable<ProductResponse>>
{
}