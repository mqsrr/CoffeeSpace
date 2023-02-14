using System.Xml;
using CoffeeSpace._ViewModels;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Messages.Requests;
using MediatR;

namespace CoffeeSpace.Messages.Handlers;

public sealed class AddToCartHandler : IRequestHandler<AddToCartRequest>
{
    private readonly CartViewModel _cartViewModel;

    public AddToCartHandler(CartViewModel cartViewModel) => _cartViewModel = cartViewModel;

    public Task<Unit> Handle(AddToCartRequest request, CancellationToken cancellationToken)
    {
        _cartViewModel.AddOrderItem(request.Item);
        
        return Unit.Task;
    }
}