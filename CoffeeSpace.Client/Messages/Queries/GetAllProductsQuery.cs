using CoffeeSpace.Client.Models.Products;
using Mediator;

namespace CoffeeSpace.Client.Messages.Queries;

public sealed class GetAllProductsQuery : IQuery<IEnumerable<Product>>
{
}