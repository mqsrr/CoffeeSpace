using CoffeeSpace.Client._ViewModels;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands.Handlers;

public sealed class AddProductToCartCommandHandler : ICommandHandler<AddProductToCartCommand>
{
    private readonly CartViewModel _cartViewModel;

    public AddProductToCartCommandHandler(CartViewModel cartViewModel)
    {
        _cartViewModel = cartViewModel;
    }

    public ValueTask<Unit> Handle(AddProductToCartCommand command, CancellationToken cancellationToken)
    {
        _cartViewModel.AddProductToCart(command.Product);

        return Unit.ValueTask;
    }
}