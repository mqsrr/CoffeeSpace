using CoffeeSpace._ViewModels;
using CoffeeSpace.Messages.Requests;
using MediatR;

namespace CoffeeSpace.Messages.Handlers;

public sealed class AddToCartRequestHandler : IRequestHandler<AddToCartRequest>
{
    private readonly CartViewModel _cartViewModel;

    public AddToCartRequestHandler(CartViewModel cartViewModel) => _cartViewModel = cartViewModel;

    public Task<Unit> Handle(AddToCartRequest request, CancellationToken cancellationToken)
    {
        _cartViewModel.AddOrderItem(request.Item);
        
        return Unit.Task;
    }
}