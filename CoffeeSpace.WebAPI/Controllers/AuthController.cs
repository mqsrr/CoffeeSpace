using CoffeeSpace.WebAPI.Dto.Requests;
using CoffeeSpace.WebAPI.Dto.Response;
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
    public async Task<IActionResult> Login([FromBody] CustomerLoginModel customer)
    {
        JwtTokenResponse token = await _accountService.LoginAsync(customer);

        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CustomerRegisterModel customer)
    {
        JwtTokenResponse token = await _accountService.RegisterAsync(customer);
        
        return Ok(token);
    }


}