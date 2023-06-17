using CoffeeSpace.IdentityApi.Application.Mapping;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.IdentityApi.Application.Messages.Commands.Handlers;

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
        var jwtToken = await _authService.RegisterAsync(user, cancellationToken);
        
        return jwtToken;
    }
}