namespace CoffeeSpace.WebAPI.Dto.Requests;

public sealed class CustomerLoginModel
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    
}