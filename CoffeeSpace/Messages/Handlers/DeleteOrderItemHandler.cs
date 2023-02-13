using CoffeeSpace._ViewModels;
using CoffeeSpace.Messages.Requests;
using MediatR;

namespace CoffeeSpace.Messages.Handlers;

public sealed class DeleteOrderItemHandler : IRequestHandler<DeleteOrderItemRequest>
{
    private readonly CartViewModel _cartViewModel;

    public DeleteOrderItemHandler(CartViewModel cartViewModel) => _cartViewModel = cartViewModel;

    public Task<Unit> Handle(DeleteOrderItemRequest request, CancellationToken cancellationToken)
    {
        _cartViewModel.OrderItems.Remove(request.Item);

        return Unit.Task;
    }
}