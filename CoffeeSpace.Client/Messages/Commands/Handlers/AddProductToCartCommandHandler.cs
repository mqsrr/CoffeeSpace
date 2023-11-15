using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Mappers;
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
        _cartViewModel.AddOrderItemIntoCart(command.Product.ToOrderItem());
        return Unit.ValueTask;
    }
}