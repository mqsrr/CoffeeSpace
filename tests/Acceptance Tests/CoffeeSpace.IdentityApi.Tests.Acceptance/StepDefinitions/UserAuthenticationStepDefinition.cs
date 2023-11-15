using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using CoffeeSpace.IdentityApi.Tests.Acceptance.Helpers;
using CoffeeSpace.IdentityApi.Tests.Acceptance.Models;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CoffeeSpace.IdentityApi.Tests.Acceptance.StepDefinitions;

[Binding]
public sealed class UserAuthenticationStepDefinition
{
    private readonly HttpClient _httpClient;
    private readonly ScenarioContext _scenarioContext;

    public UserAuthenticationStepDefinition(HttpClient httpClient, ScenarioContext scenarioContext)
    {
        _httpClient = httpClient;
        _scenarioContext = scenarioContext;
    }

    [Given(@"user with following credentials:")]
    public async Task GivenUserWithFollowingCredentials(Table table)
    {
        var registerRequest = table.CreateInstance<RegisterRequest>();
        const string requestUri = ApiEndpoints.Authentication.Register;

        await _httpClient.PostAsJsonAsync(requestUri, registerRequest);
        _scenarioContext.Add("Register Request", registerRequest);
    }

    [When(@"the POST request is sent with the following credentials:")]
    public async Task WhenThePostRequestIsSentWithTheFollowingCredentials(Table table)
    {
        var loginRequest = table.CreateInstance<LoginRequest>();
        const string requestUri = ApiEndpoints.Authentication.Login;

        var response = await _httpClient.PostAsJsonAsync(requestUri, loginRequest);
        string token = await response.Content.ReadAsStringAsync();

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Jwt Token", jwtToken);
    }

    [Then(@"the response status code should be (.*)")]
    public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
    {
        var responseStatusCode = _scenarioContext.Get<HttpStatusCode>("Response Status Code");
        responseStatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }

    [Then(@"the response should contain JWT token")]
    public void ThenTheResponseShouldContainJwtToken()
    {
        var jwtToken = _scenarioContext.Get<JwtSecurityToken>("Jwt Token");
        var registerRequest = _scenarioContext.Get<RegisterRequest>("Register Request");
        
        jwtToken.Should().NotBeNull();
        jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value.Should().BeEquivalentTo(registerRequest.Email);
    }
    
    [When(@"POST request is sent with following credentials:")]
    public async Task WhenPostRequestIsSentWithFollowingCredentials(Table table)
    {
        var registerRequest = table.CreateInstance<RegisterRequest>();
        const string requestUri = ApiEndpoints.Authentication.Register;

        var response = await _httpClient.PostAsJsonAsync(requestUri, registerRequest);
        string token = await response.Content.ReadAsStringAsync();
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        
        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Jwt Token", jwtToken);
        _scenarioContext.Add("Register Request", registerRequest);
    }
}