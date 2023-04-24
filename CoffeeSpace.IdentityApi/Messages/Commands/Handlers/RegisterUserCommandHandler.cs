using CoffeeSpace.IdentityApi.Mapping;
using CoffeeSpace.IdentityApi.Models;
using CoffeeSpace.IdentityApi.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.IdentityApi.Messages.Commands.Handlers;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, string?>
{
    private readonly IAuthService<ApplicationUser> _authService;

    public RegisterUserCommandHandler(IAuthService<ApplicationUser> authService)
    {
        _authService = authService;
    }

    public async ValueTask<string?> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = command.Request.ToUser();
        var token = await _authService.RegisterAsync(user, cancellationToken);
        
        return token;
    }
}