using CoffeeSpace.Data.Authentication.Response;
using CoffeeSpace.WebAPI.Dto.Requests;
using CoffeeSpace.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AuthController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CustomerLoginModel customer, CancellationToken cancellationToken)
    {
        JwtResponse jwtResponse = await _accountService.LoginAsync(customer, cancellationToken);

        return Ok(jwtResponse);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CustomerRegisterModel customer, CancellationToken cancellationToken)
    {
        JwtResponse jwtResponse = await _accountService.RegisterAsync(customer, cancellationToken);
        
        return Ok(jwtResponse);
    }


}