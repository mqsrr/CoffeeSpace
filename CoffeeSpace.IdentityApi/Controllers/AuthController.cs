using Asp.Versioning;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using CoffeeSpace.IdentityApi.Application.Helpers;
using CoffeeSpace.IdentityApi.Application.Messages.Commands;
using CoffeeSpace.IdentityApi.Application.Messages.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.IdentityApi.Controllers;

[ApiController]
[ApiVersion(1.0)]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(ApiEndpoints.Authentication.Register)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        string? token = await _sender.Send(new RegisterUserCommand
        {
            Request = request
        }, cancellationToken);

        return token is not null
            ? Ok(token)
            : BadRequest();
    }

    [HttpPost(ApiEndpoints.Authentication.Login)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        string? token = await _sender.Send(new LoginUserQuery
        {
            Request = request
        }, cancellationToken);

        return token is not null
            ? Ok(token)
            : BadRequest("Login credentials are incorrect");
    }
}