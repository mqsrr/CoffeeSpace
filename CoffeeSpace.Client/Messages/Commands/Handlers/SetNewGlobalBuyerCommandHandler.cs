using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.WebApiClients;
using Mediator;

namespace CoffeeSpace.Client.Messages.Commands.Handlers;

internal sealed class SetNewGlobalBuyerCommandHandler : ICommandHandler<SetNewGlobalBuyerCommand, bool>
{
    private readonly IBuyersWebApi _buyersWebApi;
    private readonly ProfileViewModel _profileViewModel;
    
    public SetNewGlobalBuyerCommandHandler(IBuyersWebApi buyersWebApi, ProfileViewModel profileViewModel)
    {
        _buyersWebApi = buyersWebApi;
        _profileViewModel = profileViewModel;
    }

    public async ValueTask<bool> Handle(SetNewGlobalBuyerCommand command, CancellationToken cancellationToken)
    {
        var buyer = await _buyersWebApi.GetBuyerByEmailAsync(command.BuyerEmail, CancellationToken.None);
        if (buyer == null)
        {
            return false;
        }

        _profileViewModel.Buyer = buyer;
        await SecureStorage.SetAsync("buyer-id", buyer.Id);
        return true;
    }
}