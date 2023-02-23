using CoffeeSpace._ViewModels;
using CoffeeSpace.Data.Models.CustomerInfo;
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
        Customer customer = authenticatedRequest.Customer;

        if (customer.PictureUrl == "default") 
            customer.PictureUrl = "user.png";

        _profileViewModel.Customer = customer;

        return Unit.Task;
    }
}