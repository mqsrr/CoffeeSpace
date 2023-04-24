using CoffeeSpace.IdentityApi.Contracts.Requests.Login;
using CoffeeSpace.IdentityApi.Contracts.Requests.Register;
using CoffeeSpace.IdentityApi.Helpers;
using CoffeeSpace.IdentityApi.Messages.Commands;
using CoffeeSpace.IdentityApi.Messages.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CoffeeSpace.IdentityApi.Controllers;

[ApiController]
[EnableRateLimiting("TokenBucket")]
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
        var token = await _sender.Send(new RegisterUserCommand
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
        var token = await _sender.Send(new LoginUserQuery
        {
            Request = request
        }, cancellationToken);

        return token is not null
            ? Ok(token)
            : BadRequest("Login credentials are incorrect");
    }
}