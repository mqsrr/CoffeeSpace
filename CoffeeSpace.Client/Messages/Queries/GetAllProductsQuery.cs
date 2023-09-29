using CoffeeSpace.Client.Models.Products;
using CoffeeSpace.Client.WebApiClients;
using Mediator;

namespace CoffeeSpace.Client.Messages.Queries;

public sealed class GetAllProductsQuery : IQuery<IEnumerable<Product>>
{
}