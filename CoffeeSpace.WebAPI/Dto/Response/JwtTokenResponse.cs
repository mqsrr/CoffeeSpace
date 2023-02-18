namespace CoffeeSpace.WebAPI.Dto.Response;

public sealed class JwtTokenResponse : IJwtTokenResponse
{
    public string Token { get; init; }

    public JwtTokenResponse(string token) => 
        Token = token;
}