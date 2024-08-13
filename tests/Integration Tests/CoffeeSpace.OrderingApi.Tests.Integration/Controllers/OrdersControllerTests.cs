using System.Net.Http.Json;
using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;
using CoffeeSpace.OrderingApi.Tests.Integration.Fixtures;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Controllers;

[Collection("Ordering Dependencies")]
public sealed class OrdersControllerTests
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
        string request = ApiEndpoints.Orders.GetAll.Replace("{buyerId:guid}", buyer.Id.ToString());
        
        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert
        var orderResponses = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponse>>();
    }
    
    [Fact]
    public async Task GetOrderByBuyerId_ShouldReturn200_AndExistingOrder()
    {
        // Arrange
        var order = _orders.First();
        var buyer = _buyers.First();
        string request = ApiEndpoints.Orders.Get
            .Replace("{buyerId:guid}", buyer.Id.ToString())
            .Replace("{id:guid}", order.Id.ToString());
        
        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert

    }
    
    [Fact]
    public async Task CreateOrder_ShouldReturn201_AndCreateOrder()
    {
        // Arrange
        var order = new CreateOrderRequest
        {
            Address = AutoFaker.Generate<CreateAddressRequest, CreateAddressRequestFaker>(),
            OrderItems = AutoFaker.Generate<OrderItem, OrderItemFaker>(11).TakeLast(1),
            Status = OrderStatus.Submitted
        };
        var buyer = _buyers.ElementAt(1);
        string request = ApiEndpoints.Orders.Create.Replace("{buyerId:guid}", buyer.Id.ToString());
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, order);

        // Assert
    }
    
    [Fact]
    public async Task DeleteOrderById_ShouldReturn200_AndDeleteOrder()
    {
        // Arrange
        var order = _orders.Last();
        string request = ApiEndpoints.Orders.Delete
            .Replace("{buyerId:guid}", order.BuyerId.ToString())
            .Replace("{id:guid}", order.Id.ToString());
        
        // Act
        var response = await _httpClient.DeleteAsync(request);

        // Assert
    }
}