using Asp.Versioning;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using CoffeeSpace.IdentityApi.Application.Helpers;
using CoffeeSpace.IdentityApi.Application.Mapping;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using CoffeeSpace.IdentityApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.IdentityApi.Controllers;

[ApiController]
[ApiVersion(1.0)]
[ServiceFilter(typeof(ApiKeyAuthorizationFilter))]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService<ApplicationUser> _authService;

    public AuthController(IAuthService<ApplicationUser> authService)
    {
        _authService = authService;
    }

    [HttpPost(ApiEndpoints.Authentication.Register)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var applicationUser = request.ToUser();
        string? token = await _authService.RegisterAsync(applicationUser, cancellationToken);
        
        return token is not null
            ? Ok(token)
            : BadRequest();
    }

    [HttpPost(ApiEndpoints.Authentication.Login)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        string? token = await _authService.LoginAsync(request.Username, request.Password, cancellationToken);
        return token is not null
            ? Ok(token)
            : BadRequest("Login credentials are incorrect");
    }
}