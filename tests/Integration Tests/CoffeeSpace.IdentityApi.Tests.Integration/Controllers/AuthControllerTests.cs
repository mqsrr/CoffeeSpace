using System.Net;
using System.Net.Http.Json;
using AutoBogus;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using CoffeeSpace.IdentityApi.Application.Helpers;
using CoffeeSpace.IdentityApi.Tests.Integration.Fakers;
using CoffeeSpace.IdentityApi.Tests.Integration.Fixtures;
using FluentAssertions;
using Xunit;

namespace CoffeeSpace.IdentityApi.Tests.Integration.Controllers;

public sealed class AuthControllerTests : IClassFixture<IdentityApiFactory>
{
    private readonly HttpClient _httpClient;
    private readonly IdentityApiFactory _apiFactory;
    
    public AuthControllerTests(IdentityApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _apiFactory = apiFactory;
    }

    [Fact]
    public async Task Register_ShouldReturn200_AndJWTToken_WhenUserIsAuthenticated()
    {
        // Arrange
        const string request = ApiEndpoints.Authentication.Register;
        var userToCreate = AutoFaker.Generate<RegisterRequest, RegisterRequestFaker>();
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, userToCreate);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Register_ShouldReturn400_WithoutJWTToken_WhenUserIsNotAuthenticated()
    {
        // Arrange
        const string request = ApiEndpoints.Authentication.Register;
        var userToCreate = AutoFaker.Generate<RegisterRequest, FaultRegisterRequestFaker>();
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, userToCreate);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Login_ShouldReturn200_AndJWToken_WhenUserIsAuthenticated()
    {
        // Arrange

        var userToCreate = AutoFaker.Generate<RegisterRequest, RegisterRequestFaker>();
        await _httpClient.PostAsJsonAsync(ApiEndpoints.Authentication.Register, userToCreate);
        
        const string request = ApiEndpoints.Authentication.Login;
        var userToLogin = new LoginRequest
        {
            Username = userToCreate.UserName,
            Password = userToCreate.Password,
        };
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, userToLogin);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Login_ShouldReturn400_WithoutJWToken_WhenUserIsNotAuthenticated()
    {
        // Arrange
        const string request = ApiEndpoints.Authentication.Login;
        var userToLogin = new LoginRequest
        {
            Username = AutoFaker.Generate<string>(),
            Password = AutoFaker.Generate<string>(),
        };
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, userToLogin);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }    
    
    [Fact]
    public async Task RequestWithoutAPIKey_ShouldReturn401()
    {
        // Arrange
        var testableHttpClient = _apiFactory.CreateClient();
        testableHttpClient.DefaultRequestHeaders.Remove("X-Api-Key");
        
        const string request = ApiEndpoints.Authentication.Register;
        var userToCreate = AutoFaker.Generate<RegisterRequest, RegisterRequestFaker>();
        
        // Act
        var response = await testableHttpClient.PostAsJsonAsync(request, userToCreate);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task RequestWithIncorrectAPIKey_ShouldReturn401()
    {
        // Arrange
        var testableHttpClient = _apiFactory.CreateClient();
        testableHttpClient.DefaultRequestHeaders.Clear();
        
        testableHttpClient.DefaultRequestHeaders.Remove("X-Api-Key");
        testableHttpClient.DefaultRequestHeaders.Add("X-Api-Key", "invalid");
        
        const string request = ApiEndpoints.Authentication.Login;
        var userToCreate = AutoFaker.Generate<RegisterRequest, RegisterRequestFaker>();
        var userToLogin = new LoginRequest
        {
            Username = userToCreate.UserName,
            Password = userToCreate.Password,
        };
        
        // Act
        var response = await testableHttpClient.PostAsJsonAsync(request, userToLogin);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}