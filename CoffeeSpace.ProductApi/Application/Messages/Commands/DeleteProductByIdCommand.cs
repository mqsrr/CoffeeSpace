using Mediator;

namespace CoffeeSpace.ProductApi.Application.Messages.Commands;

public sealed class DeleteProductByIdCommand : ICommand<bool>
{
    public required string Id { get; init; }
}