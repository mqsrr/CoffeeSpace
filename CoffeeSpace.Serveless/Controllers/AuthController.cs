using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.Serveless.Controllers;


[ApiController]
[Route("[controller]")]
public sealed class AuthController : ControllerBase
{
    private ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }
    
    
}