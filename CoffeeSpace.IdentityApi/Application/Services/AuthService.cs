using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using CoffeeSpace.Messages.Buyers;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.IdentityApi.Application.Services;

internal sealed class AuthService : IAuthService<ApplicationUser>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenWriter<ApplicationUser> _tokenWriter;
    private readonly ISendEndpointProvider _endpointProvider;
    private readonly ILogger _logger;

    public AuthService(SignInManager<ApplicationUser> signInManager, ITokenWriter<ApplicationUser> tokenWriter, ISendEndpointProvider endpointProvider)
    {
        _signInManager = signInManager;
        _tokenWriter = tokenWriter;
        _endpointProvider = endpointProvider;
        _logger = signInManager.Logger;
    }

    public async Task<string?> RegisterAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var result = await _signInManager.UserManager.CreateAsync(user, user.Password);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Account cannot be created using the {@Username} username. Result errors: {@Errors}", user.UserName, result.Errors);
            return null;
        }

        var registerResult = await _signInManager.PasswordSignInAsync(user.UserName!, user.Password, false, false);
        if (!registerResult.Succeeded)
        {
            _logger.LogWarning("Unexpected error occurred while signing in with password using {@Username} username, IsNotAllowed-{@IsNotAllowed}", user.UserName, registerResult.IsNotAllowed.ToString());
            return null;
        }

        var sendEndpoint = await _endpointProvider.GetSendEndpoint(new Uri("queue:register-new-buyer"));
        await sendEndpoint.Send<RegisterNewBuyer>(new
        {
            Name = user.UserName,
            Email = user.Email
        }, cancellationToken);

        var claims = await _signInManager.CreateUserPrincipalAsync(user);
        await _signInManager.UserManager.AddClaimsAsync(user, claims.Claims);
        
        string token = await _tokenWriter.WriteTokenAsync(user, cancellationToken);
        return token;
    }

    public Task<string?> LoginAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return LoginAsync(user.UserName!, user.Password, cancellationToken);
    }

    public async Task<string?> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, false);
        if (!signInResult.Succeeded)
        {
            _logger.LogWarning("Unexpected error occurred while signing in with password using {@Username} username, IsNotAllowed-{@IsNotAllowed}", username, signInResult.IsNotAllowed.ToString());
            return null;
        }

        var applicationUser = await _signInManager.UserManager.FindByNameAsync(username);
        string token = await _tokenWriter.WriteTokenAsync(applicationUser!, cancellationToken);
        
        return token;
    }
}