using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.ViewModels;
using Mediator;

namespace CoffeeSpace.AClient.Messages.Commands.Handlers;

public sealed class AddToCartCommandHandler : ICommandHandler<AddToCartCommand>
{
    private readonly CartWindowViewModel _cartWindowViewModel;

    public AddToCartCommandHandler(CartWindowViewModel cartWindowViewModel)
    {
        _cartWindowViewModel = cartWindowViewModel;
    }

    public ValueTask<Unit> Handle(AddToCartCommand command, CancellationToken cancellationToken)
    {
        var addedProduct = command.Product;
        bool isContains = _cartWindowViewModel.CartProducts.Contains(addedProduct);
        
        if (!isContains)
        {
            _cartWindowViewModel.CartProducts.Add(addedProduct);
            return Unit.ValueTask;
        }

        addedProduct.Quantity++;
        return Unit.ValueTask;
    }
}