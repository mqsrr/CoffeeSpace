using System.Collections.Generic;
using CoffeeSpace.AClient.Models;
using Mediator;

namespace CoffeeSpace.AClient.Messages.Queries;

public sealed class GetAllProductsQuery : IQuery<IEnumerable<Product>>
{
    
}