using System.Xml;
using CoffeeSpace._ViewModels;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Messages.Requests;
using MediatR;

namespace CoffeeSpace.Messages.Handlers;

public sealed class AddToCartHandler : IRequestHandler<AddToCartRequest>
{
    private readonly IServiceScope _serviceScope;

    public AddToCartHandler(IServiceScopeFactory serviceFactory) => _serviceScope = serviceFactory.CreateScope();

    public Task<Unit> Handle(AddToCartRequest request, CancellationToken cancellationToken)
    {
        CartViewModel cartViewModel = _serviceScope.ServiceProvider.GetService<CartViewModel>();

        cartViewModel.AddOrderItem(request.Item);
        
        return Unit.Task;
    }
}