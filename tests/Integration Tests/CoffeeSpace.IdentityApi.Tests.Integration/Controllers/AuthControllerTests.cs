using System.Net.Http.Json;
using AutoBogus;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using CoffeeSpace.IdentityApi.Application.Helpers;
using CoffeeSpace.IdentityApi.Tests.Integration.Fakers;
using CoffeeSpace.IdentityApi.Tests.Integration.Fixtures;
using MassTransit.Testing;

namespace CoffeeSpace.IdentityApi.Tests.Integration.Controllers;

[UsesVerify]
public sealed class AuthControllerTests : IClassFixture<IdentityApiFactory>, IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly ITestHarness _testHarness;
    
    public AuthControllerTests(IdentityApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _testHarness = apiFactory.Services.GetTestHarness();
    }

    [Fact]
    public async Task Register_ShouldReturn200WithJwtToken_AndRegisterNewUser()
    {
        // Arrange
        const string request = ApiEndpoints.Authentication.Register;
        var userToCreate = AutoFaker.Generate<RegisterRequest, RegisterRequestFaker>();
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, userToCreate);

        // Assert
        var sqlLogs =Recording.Stop("IdentityDb");
        await Verify(new
        {
            response,
            _testHarness,
            sqlLogs
        }).IgnoreMembers("Parameters", "Cookie", "Set-Cookie");
    }
    
    [Fact]
    public async Task Login_ShouldReturn200WithJwtToken()
    {
        // Arrange
        var registerUserRequest = AutoFaker.Generate<RegisterRequest, RegisterRequestFaker>(2).Last();
        await _httpClient.PostAsJsonAsync(ApiEndpoints.Authentication.Register, registerUserRequest);
        
        const string request = ApiEndpoints.Authentication.Login;
        var userToCreate = AutoFaker.Generate<LoginRequest, LoginRequestFaker>(builder =>
            builder.WithArgs(registerUserRequest.UserName, registerUserRequest.Password));
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, userToCreate);

        // Assert
        var sqlLogs =Recording.Stop("IdentityDb");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Parameters", "Cookie", "Set-Cookie");
    }

    public Task InitializeAsync()
    {
        Recording.Start("IdentityDb");
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}