using System.Net;
using System.Net.Http.Json;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Helpers;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Models;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CoffeeSpace.OrderingApi.Tests.Acceptance.StepDefinitions;

[Binding]
public sealed class BuyersManagementStepDefinition
{
    private readonly HttpClient _httpClient;
    private readonly ScenarioContext _scenarioContext;

    public BuyersManagementStepDefinition(HttpClient httpClient, ScenarioContext scenarioContext)
    {
        _httpClient = httpClient;
        _scenarioContext = scenarioContext;
    }

    [When(@"a POST request is sent to create a new buyer")]
    public async Task WhenApostRequestIsSentToCreateANewBuyer(Table table)
    {
        var createBuyerRequest = table.CreateInstance<CreateBuyerRequest>();
        string requestUri = ApiEndpoints.Buyer.Create();
        
        var response = await _httpClient.PostAsJsonAsync(requestUri, createBuyerRequest);
        var createdBuyer = await response.Content.ReadFromJsonAsync<BuyerResponse>();
        
        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Created Buyer", createdBuyer);
    }

    [Then(@"the buyer is created successfully")]
    public async Task ThenTheBuyerIsCreatedSuccessfully()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        string requestUri = ApiEndpoints.Buyer.Get(createdBuyer.Id);

        var buyerResponse = await _httpClient.GetFromJsonAsync<BuyerResponse>(requestUri);
        buyerResponse.Should().BeEquivalentTo(createdBuyer, options => options.Excluding(response => response.Orders));
    }

    [When(@"a GET request is sent to retrieve buyer by ID")]
    public async Task WhenAgetRequestIsSentToRetrieveBuyerById()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        string requestUri = ApiEndpoints.Buyer.Get(createdBuyer.Id);

        var response = await _httpClient.GetAsync(requestUri);
        var buyerResponse = await response.Content.ReadFromJsonAsync<BuyerResponse>();
        
        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Retrieved Buyer", buyerResponse);
    }

    [When(@"a GET request is sent to retrieve buyer by Email")]
    public async Task WhenAgetRequestIsSentToRetrieveBuyerByEmail()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        string requestUri = ApiEndpoints.Buyer.GetWithEmail(createdBuyer.Email);

        var response = await _httpClient.GetAsync(requestUri);
        var buyerResponse = await response.Content.ReadFromJsonAsync<BuyerResponse>();
        
        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Retrieved Buyer", buyerResponse);
    }

    [Then(@"the buyer response should be with correct properties")]
    public void ThenTheBuyerResponseShouldBeWithCorrectProperties()
    {
        var buyerResponse = _scenarioContext.Get<BuyerResponse>("Retrieved Buyer");
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");

        buyerResponse.Should().BeEquivalentTo(createdBuyer, options => options.Excluding(response => response.Orders));
    }

    [When(@"PUT request is sent to update the buyer with updated buyer's data:")]
    public async Task WhenPutRequestIsSentToUpdateTheBuyerWithUpdatedBuyersData(Table table)
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        var updateBuyerRequest = table.CreateInstance<UpdateBuyerRequest>();
        string requestUri = ApiEndpoints.Buyer.Update(createdBuyer.Id);
        
        var response = await _httpClient.PutAsJsonAsync(requestUri, updateBuyerRequest);
        var updatedBuyer = await response.Content.ReadFromJsonAsync<BuyerResponse>();

        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Updated Buyer", updatedBuyer);
    }

    [Then(@"a GET request to retrieve the updated buyer should return the correct updated buyer's information")]
    public async Task ThenAgetRequestToRetrieveTheUpdatedBuyerShouldReturnTheCorrectUpdatedBuyersInformation()
    {
        var updatedBuyer = _scenarioContext.Get<BuyerResponse>("Updated Buyer");
        string requestUri = ApiEndpoints.Buyer.Get(updatedBuyer.Id);

        var buyerResponse = await _httpClient.GetFromJsonAsync<BuyerResponse>(requestUri);
        buyerResponse.Should().BeEquivalentTo(updatedBuyer, options => options.Excluding(response => response.Orders));
    }

    [When(@"DELETE request is sent to delete buyer By ID:")]
    public async Task WhenDeleteRequestIsSentToDeleteBuyerById()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        string requestUri = ApiEndpoints.Buyer.Delete(createdBuyer.Id);

        var response = await _httpClient.DeleteAsync(requestUri);
        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Deleted Buyer's Id", createdBuyer.Id);
    }

    [Then(@"the buyer should be deleted")]
    public async Task ThenTheBuyerShouldBeDeleted()
    {
        string buyerId = _scenarioContext.Get<string>("Deleted Buyer's Id");
        string requestUri = ApiEndpoints.Buyer.Get(buyerId);

        var response = await _httpClient.GetAsync(requestUri);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}