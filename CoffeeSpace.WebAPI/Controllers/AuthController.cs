using AutoMapper;
using CoffeeSpace.Application;
using CoffeeSpace.Application.Authentication.Response;
using CoffeeSpace.Contracts.Requests.Customer;
using CoffeeSpace.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.WebAPI.Controllers;

[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    public AuthController(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }
    
    [HttpPost(ApiEndpoints.Auth.Login)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        JwtResponse jwtResponse = await _accountService.LoginAsync(request, cancellationToken);

        return Ok(jwtResponse);
    }

    [HttpPost(ApiEndpoints.Auth.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        JwtResponse jwtResponse = await _accountService.RegisterAsync(request, cancellationToken);
        
        return Ok(jwtResponse);
    }


}