using System.Net;
using System.Net.Http.Json;
using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Mapping;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;
using CoffeeSpace.OrderingApi.Tests.Integration.Fixtures;
using FluentAssertions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Controllers;

[Collection("Ordering Dependencies")]
public sealed class OrdersControllerTests
{
    private readonly HttpClient _httpClient;
    private readonly List<Buyer> _buyers;

    public OrdersControllerTests(OrderingApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _buyers = apiFactory.Buyers.ToList();
    }

    [Fact]
    public async Task GetAllOrdersByBuyerId_ShouldReturn200_WhenBuyerExists()
    {
        // Arrange
        var buyer = _buyers.First();
        string request = ApiEndpoints.Orders.GetAll.Replace("{buyerId:guid}", buyer.Id.ToString());

        var expectedResponse = buyer.Orders!.Select(order => order.ToResponse());

        // Act
        var response = await _httpClient.GetAsync(request);
        var orderResponses = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        orderResponses.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetAllOrdersByBuyerId_ShouldReturn200_AndEmptyList_WhenBuyerDoesNotExist_OrDoesNotHaveOrders()
    {
        // Arrange
        var buyer = _buyers[1];
        string request = ApiEndpoints.Orders.GetAll.Replace("{buyerId:guid}", buyer.Id.ToString());

        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOrderByBuyerId_ShouldReturn200_WhenOrderExists()
    {
        // Arrange
        var buyer = _buyers.First();
        var order = buyer.Orders!.First();

        var expectedResponse = order.ToResponse();
        string request = ApiEndpoints.Orders.Get
            .Replace("{buyerId:guid}", buyer.Id.ToString())
            .Replace("{id:guid}", order.Id.ToString());

        // Act
        var response = await _httpClient.GetAsync(request);
        var orderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        orderResponse.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetOrderByBuyerId_ShouldReturn404_WhenOrderDoesNotExist()
    {
        // Arrange
        var buyer = _buyers.First();
        var randomOrderId = Guid.NewGuid();

        string request = ApiEndpoints.Orders.Get
            .Replace("{buyerId:guid}", buyer.Id.ToString())
            .Replace("{id:guid}", randomOrderId.ToString());

        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn201_WhenOrderIsCreated()
    {
        // Arrange
        var order = AutoFaker.Generate<CreateOrderRequest, CreateOrderRequestFaker>();
        var buyer = _buyers[1];

        var expectedResponse = order.ToOrder(buyer.Id).ToResponse();
        string request = ApiEndpoints.Orders.Create.Replace("{buyerId:guid}", buyer.Id.ToString());

        // Act
        var response = await _httpClient.PostAsJsonAsync(request, order);
        var orderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        orderResponse.Should()
            .BeEquivalentTo(expectedResponse, 
                options => options.Excluding(r => r.Id).Excluding(r => r.Address.Id));
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn400_WhenOrderIsNotCreated()
    {
        // Arrange
        var buyer = _buyers[1];
        string request = ApiEndpoints.Orders.Create.Replace("{buyerId:guid}", buyer.Id.ToString());

        // Act
        var response = await _httpClient.PostAsJsonAsync<CreateOrderRequest>(request, null!);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteOrderById_ShouldReturn204_WhenOrderIsDeleted()
    {
        // Arrange
        var buyer = _buyers.First();
        var createOrderRequestBody = AutoFaker.Generate<CreateOrderRequest, CreateOrderRequestFaker>();
        string createOrderRequest = ApiEndpoints.Orders.Create.Replace("{buyerId:guid}", buyer.Id.ToString());

        var response = await _httpClient.PostAsJsonAsync(createOrderRequest, createOrderRequestBody);
        var createdOrderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>();

        string request = ApiEndpoints.Orders.Delete
            .Replace("{buyerId:guid}", buyer.Id.ToString())
            .Replace("{id:guid}", createdOrderResponse!.Id.ToString());

        // Act
        response = await _httpClient.DeleteAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteOrderById_ShouldReturn404_WhenOrderIsNotDeleted()
    {
        // Arrange
        var order = AutoFaker.Generate<Order, OrderFaker>(builder => builder.WithArgs(Guid.NewGuid()));

        string request = ApiEndpoints.Orders.Delete
            .Replace("{buyerId:guid}", order.BuyerId.ToString())
            .Replace("{id:guid}", order.Id.ToString());

        // Act
        var response = await _httpClient.DeleteAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}