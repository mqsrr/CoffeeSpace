using CoffeeSpace._ViewModels;
using CoffeeSpace.Messages.Requests;
using MediatR;

namespace CoffeeSpace.Messages.Handlers;

public sealed class AuthenticateCustomerRequestHandler : IRequestHandler<CustomerAuthenticatedRequest>
{
    private readonly ProfileViewModel _profileViewModel;

    public AuthenticateCustomerRequestHandler(ProfileViewModel profileViewModel) => 
        _profileViewModel = profileViewModel;
    
    public Task<Unit> Handle(CustomerAuthenticatedRequest authenticatedRequest, CancellationToken cancellationToken)
    {
        _profileViewModel.Customer = authenticatedRequest.Customer;

        return Unit.Task;
    }
}