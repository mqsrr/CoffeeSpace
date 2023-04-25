using CoffeeSpace.Domain.Products;
using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Queries;

public sealed class GetProductByIdQuery : IQuery<Product>
{
    public required string Id { get; init; }
}