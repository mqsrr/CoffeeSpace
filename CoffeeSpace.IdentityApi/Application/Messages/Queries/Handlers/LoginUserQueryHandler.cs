using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using Mediator;

namespace CoffeeSpace.IdentityApi.Application.Messages.Queries.Handlers;

internal sealed class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, string?>
{
    private readonly IAuthService<ApplicationUser> _authService;

    public LoginUserQueryHandler(IAuthService<ApplicationUser> authService)
    {
        _authService = authService;
    }

    public async ValueTask<string?> Handle(LoginUserQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var token = await _authService.LoginAsync(request.Username, request.Password, cancellationToken);

        return token;
    }
}