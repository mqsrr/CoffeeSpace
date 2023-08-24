using System.Net.Http.Json;
using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;
using CoffeeSpace.OrderingApi.Tests.Integration.Fixtures;
using VerifyTests.EntityFramework;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Controllers;

[UsesVerify]
[Collection("Ordering Dependencies")]
public sealed class OrdersControllerTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly IEnumerable<Order> _orders;
    private readonly IEnumerable<Buyer> _buyers;
    
    public OrdersControllerTests(OrderingApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _orders = apiFactory.Orders;
        _buyers = apiFactory.Buyers;
    }

    [Fact]
    public async Task GetAllOrdersByBuyerId_ShouldReturn200_AndAllBuyersOrders()
    {
        // Arrange
        var buyer = _buyers.First();
        string request = ApiEndpoints.Orders.GetAll.Replace("{buyerId:guid}", buyer.Id);
        
        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert
        var sqlLogs = EfRecording.FinishRecording("OrderingDb");
        var orderResponses = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponse>>();
        await Verify(new
        {
            response, 
            orderResponses,
            sqlLogs
        }).IgnoreMember("Authorization");
    }
    
    [Fact]
    public async Task GetOrderByBuyerId_ShouldReturn200_AndExistingOrder()
    {
        // Arrange
        var order = _orders.First();
        var buyer = _buyers.First();
        string request = ApiEndpoints.Orders.Get
            .Replace("{buyerId:guid}", buyer.Id)
            .Replace("{id:guid}", order.Id);
        
        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert
        var sqlLogs = EfRecording.FinishRecording("OrderingDb");

        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMember("Authorization");
    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturn201_AndCreateOrder()
    {
        // Arrange
        var order = AutoFaker.Generate<CreateOrderRequest, CreateOrderRequestFaker>();
        var buyer = _buyers.ElementAt(1);
        
        string request = ApiEndpoints.Orders.Create.Replace("{buyerId:guid}", buyer.Id);
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, order);

        // Assert
        var sqlLogs = EfRecording.FinishRecording("OrderingDb");

        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Authorization", "Host", "Exception", "Location", "Parameters");
    }
    
    [Fact]
    public async Task DeleteOrderById_ShouldReturn200_AndDeleteOrder()
    {
        // Arrange
        var order = _orders.Last();
        
        string request = ApiEndpoints.Orders.Delete
            .Replace("{buyerId:guid}", order.BuyerId)
            .Replace("{id:guid}", order.Id);
        
        // Act
        var response = await _httpClient.DeleteAsync(request);

        // Assert
        var sqlLogs = EfRecording.FinishRecording("OrderingDb");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Authorization");
    }

    public Task InitializeAsync()
    {
        EfRecording.StartRecording("OrderingDb");
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}