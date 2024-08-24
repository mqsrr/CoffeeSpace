using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Models;
using Mediator;

namespace CoffeeSpace.AClient.Messages.Queries.Handlers;

public sealed class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    public ValueTask<IEnumerable<Product>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        Console.WriteLine("Hell ya");
        return ValueTask.FromResult(Enumerable.Repeat(new Product
        {
            Id = "Cool",
            Image = null!,
            Title = "Capuchino",
            Description = "Desciption",
            UnitPrice = 10,
            Quantity = 1
        },10));
    }
}