using System.Net.Http.Json;
using CoffeeSpace.Application;
using CoffeeSpace.Application.Authentication.Response;
using CoffeeSpace.Dto;

namespace CoffeeSpace.Services;

public sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        
        string basePath = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5109/"
            : "https://localhost:7194/";

        _httpClient.BaseAddress = new Uri(basePath);
    }
    
    public async Task<JwtResponse> LoginAsync(CustomerLoginModel loginModel, CancellationToken token = default)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(ApiEndpoints.Auth.Login, loginModel, cancellationToken: token);

        if (!response.IsSuccessStatusCode)
            return new JwtResponse { IsSuccess = false };

        JwtResponse jwtResponse = await response.Content.ReadFromJsonAsync<JwtResponse>(cancellationToken: token);
        
        return jwtResponse;
    }

    public async Task<JwtResponse> RegisterAsync(CustomerRegisterModel registerModel, CancellationToken token = default)
    {
        HttpResponseMessage response =
            await _httpClient.PostAsJsonAsync(ApiEndpoints.Auth.Register, registerModel, cancellationToken: token);

        if (!response.IsSuccessStatusCode)
            return new JwtResponse { IsSuccess = false };

        JwtResponse jwtResponse = await response.Content.ReadFromJsonAsync<JwtResponse>(cancellationToken: token);

        return jwtResponse;
    }
}