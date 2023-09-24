using System.Net;
using System.Net.Http.Json;
using AutoBogus;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Fakers;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Helpers;
using CoffeeSpace.OrderingApi.Tests.Acceptance.Models;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CoffeeSpace.OrderingApi.Tests.Acceptance.StepDefinitions;

[Binding]
public sealed class OrderManagementStepDefinition
{
    private readonly HttpClient _httpClient;
    private readonly ScenarioContext _scenarioContext;

    public OrderManagementStepDefinition(HttpClient httpClient, ScenarioContext scenarioContext)
    {
        _httpClient = httpClient;
        _scenarioContext = scenarioContext;
    }
    
    [Given(@"existing buyer with the following details:")]
    public async Task GivenExistingBuyerWithTheFollowingDetails(Table table)
    {
        var createBuyerRequest = table.CreateInstance<CreateBuyerRequest>();
        string requestUri = ApiEndpoints.Buyer.Create();
        
        var response = await _httpClient.PostAsJsonAsync(requestUri, createBuyerRequest);
        var createdBuyer = await response.Content.ReadFromJsonAsync<BuyerResponse>();
        
        _scenarioContext.Add("Created Buyer", createdBuyer);
    }

    [Given(@"existing orders in the system")]
    public async Task GivenExistingOrdersInTheSystem()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        var createOrderRequests = AutoFaker.Generate<CreateOrderRequest, CreateOrderRequestFaker>(3);
        string requestUri = ApiEndpoints.Orders.Create(createdBuyer.Id);
        
        var createdOrders = new List<OrderResponse>();
        foreach (var createOrderRequest in createOrderRequests)
        {
            var response = await _httpClient.PostAsJsonAsync(requestUri, createOrderRequest);
            var createdOrder = await response.Content.ReadFromJsonAsync<OrderResponse>();

            createdOrders.Add(createdOrder!);
        }
        
        _scenarioContext.Add("Created Orders", createdOrders);
    }

    [When(@"a POST request is sent to create a new order")]
    public async Task WhenApostRequestIsSentToCreateANewOrder()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        var createOrderRequest = AutoFaker.Generate<CreateOrderRequest, CreateOrderRequestFaker>();
        string requestUri = ApiEndpoints.Orders.Create(createdBuyer.Id);
        
        var response = await _httpClient.PostAsJsonAsync(requestUri, createOrderRequest);
        var createdOrderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>();
        
        _scenarioContext.Add("Created Order Response", createdOrderResponse);
        _scenarioContext.Add("Response Status Code", response.StatusCode);
    }

    [Then(@"the order is created successfully")]
    public async Task ThenTheOrderIsCreatedSuccessfully()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        var createdOrder = _scenarioContext.Get<OrderResponse>("Created Order Response");
        string requestUri = ApiEndpoints.Orders.Get(createdBuyer.Id, createdOrder.Id);

        var orderResponse = await _httpClient.GetFromJsonAsync<OrderResponse>(requestUri);
        orderResponse.Should().BeEquivalentTo(createdOrder);
    }

    [Then(@"the response status code should be (.*)")]
    public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
    {
        var actualStatusCode = _scenarioContext.Get<HttpStatusCode>("Response Status Code");
        actualStatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }

    [When(@"a GET request is sent to retrieve order")]
    public async Task WhenAgetRequestIsSentToRetrieveOrder()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        var createdOrder = _scenarioContext.Get<List<OrderResponse>>("Created Orders").First();
        string requestUri = ApiEndpoints.Orders.Get(createdBuyer.Id, createdOrder.Id);

        var response = await _httpClient.GetAsync(requestUri);
        var retrievedOrderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>();

        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Retrieved Order", retrievedOrderResponse);
    }

    [Then(@"the order response should be with correct properties")]
    public void ThenTheOrderResponseShouldBeWithCorrectProperties()
    {
        var retrievedOrder = _scenarioContext.Get<OrderResponse>("Retrieved Order");
        var createdOrder = _scenarioContext.Get<List<OrderResponse>>("Created Orders").First();

        retrievedOrder.Should().BeEquivalentTo(createdOrder);
    }
    
    [When(@"a GET request is sent to retrieve orders by buyer ID")]
    public async Task WhenAgetRequestIsSentToRetrieveOrdersByBuyerId()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        string getAllOrdersEndpointUri = ApiEndpoints.Orders.GetAll(createdBuyer.Id);

        var response = await _httpClient.GetAsync(getAllOrdersEndpointUri);
        var ordersResponse = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponse>>();

        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Orders Response", ordersResponse);
    }

    [Then(@"the orders response should be with correct properties")]
    public void ThenTheOrdersResponseShouldBeWithCorrectProperties()
    {
        var ordersResponse = _scenarioContext.Get<IEnumerable<OrderResponse>>("Orders Response");
        var initialOrders = _scenarioContext.Get<List<OrderResponse>>("Created Orders");

        ordersResponse.Should().BeEquivalentTo(initialOrders);
    }

    [When(@"a DELETE request is sent to DELETE order by buyer ID and order ID")]
    public async Task WhenAdeleteRequestIsSentToDeleteOrderByBuyerIdAndOrderId()
    {
        var orderToDelete = _scenarioContext.Get<List<OrderResponse>>("Created Orders").First();
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        
        string requestUri = ApiEndpoints.Orders.Delete(createdBuyer.Id, orderToDelete.Id);
        var response = await _httpClient.DeleteAsync(requestUri);
        
        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Deleted Order's Id", orderToDelete.Id);
    }

    [Then(@"the order will be deleted")]
    public async Task ThenTheOrderWillBeDeleted()
    {
        var createdBuyer = _scenarioContext.Get<BuyerResponse>("Created Buyer");
        string orderId = _scenarioContext.Get<string>("Deleted Order's Id");
        
        string requestUri = ApiEndpoints.Orders.Get(createdBuyer.Id, orderId);
        var response = await _httpClient.GetAsync(requestUri);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}