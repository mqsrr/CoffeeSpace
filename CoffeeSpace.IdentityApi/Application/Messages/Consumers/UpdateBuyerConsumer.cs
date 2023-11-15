using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.Messages.Buyers;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.IdentityApi.Application.Messages.Consumers;

internal sealed class UpdateBuyerConsumer : IConsumer<UpdateBuyer>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateBuyerConsumer(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Consume(ConsumeContext<UpdateBuyer> context)
    {
        var user = await _userManager.FindByEmailAsync(context.Message.Buyer.Email);
        if (user is not null)
        {
            await _userManager.SetEmailAsync(user, context.Message.Buyer.Email);
            await _userManager.SetUserNameAsync(user, context.Message.Buyer.Name);
            
            _userManager.Logger.LogInformation("{@Username} has changed their credentials", user.UserName);
        }
    }
}