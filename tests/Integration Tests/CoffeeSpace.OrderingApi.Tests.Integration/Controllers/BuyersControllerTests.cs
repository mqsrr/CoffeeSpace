using System.Net;
using System.Net.Http.Json;
using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Buyers;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Mapping;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;
using CoffeeSpace.OrderingApi.Tests.Integration.Fixtures;
using FluentAssertions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Controllers;

[Collection("Ordering Dependencies")]
public sealed class BuyersControllerTests
{
    private readonly HttpClient _httpClient;
    private readonly List<Buyer> _buyers;

    public BuyersControllerTests(OrderingApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _buyers = apiFactory.Buyers.ToList();
    }

    [Fact]
    public async Task GetById_ShouldReturn200_WhenBuyerExists()
    {
        // Arrange
        var buyer = _buyers.First();
        var expectedResponse = buyer.ToResponse();
        string request = ApiEndpoints.Buyer.Get.Replace("{id:guid}", buyer.Id.ToString());

        // Act
        var response = await _httpClient.GetAsync(request);
        var buyerResponse = await response.Content.ReadFromJsonAsync<BuyerResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        buyerResponse.Should().BeEquivalentTo(expectedResponse);
    }
    
    [Fact]
    public async Task GetById_ShouldReturn404_WhenBuyerDoesNotExist()
    {
        // Arrange
        var randomId = Guid.NewGuid();
        string request = ApiEndpoints.Buyer.Get.Replace("{id:guid}", randomId.ToString());

        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetByEmail_ShouldReturn200_WhenBuyerExists()
    {
        // Arrange
        var buyer = _buyers.First();
        var expectedResponse = buyer.ToResponse();
        string request = ApiEndpoints.Buyer.GetWithEmail.Replace("{email}", buyer.Email);
        
        // Act
        var response = await _httpClient.GetAsync(request);
        var buyerResponse = await response.Content.ReadFromJsonAsync<BuyerResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        buyerResponse.Should().BeEquivalentTo(expectedResponse);
    }
    
    [Fact]
    public async Task GetByEmail_ShouldReturn404_WhenBuyerDoesNotExist()
    {
        // Arrange
        string randomEmail = AutoFaker.Generate<string>();
        string request = ApiEndpoints.Buyer.GetWithEmail.Replace("{email}", randomEmail);

        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturn201_WhenBuyerIsCreated()
    {
        // Arrange
        const string request = ApiEndpoints.Buyer.Create;
        var buyerToCreate = AutoFaker.Generate<CreateBuyerRequest, CreateBuyerRequestFaker>();
        var expectedResponse = buyerToCreate.ToBuyer();
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, buyerToCreate);
        var buyerResponse = await response.Content.ReadFromJsonAsync<BuyerResponse>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        buyerResponse.Should().BeEquivalentTo(expectedResponse, options => options.Excluding(buyer => buyer.Id));
    }
    
    [Fact]
    public async Task Create_ShouldReturn400_WhenBuyerIsInvalid()
    {
        // Arrange
        const string request = ApiEndpoints.Buyer.Create;
        var buyerToCreate = AutoFaker.Generate<CreateBuyerRequest, FaultCreateBuyerRequest>();
        
        // Act
        var response = await _httpClient.PostAsJsonAsync(request, buyerToCreate);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_ShouldReturn200_WhenBuyerIsUpdated()
    {
        // Arrange
        var buyer = _buyers[1];
        var updatedBuyer = AutoFaker.Generate<UpdateBuyerRequest, UpdateBuyerRequestFaker>();
        var expectedBuyer = updatedBuyer.ToBuyer(buyer.Id);
        string request = ApiEndpoints.Buyer.Update.Replace("{id:guid}", buyer.Id.ToString());
        
        // Act
        var response = await _httpClient.PutAsJsonAsync(request, updatedBuyer);
        var buyerResponse = await response.Content.ReadFromJsonAsync<BuyerResponse>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        buyerResponse.Should().BeEquivalentTo(expectedBuyer);
    }
    
    [Fact]
    public async Task Update_ShouldReturn404_WhenBuyerDoesNotExist()
    {
        // Arrange
        var randomId = Guid.NewGuid();
        var updatedBuyer = AutoFaker.Generate<UpdateBuyerRequest, UpdateBuyerRequestFaker>();
        string request = ApiEndpoints.Buyer.Update.Replace("{id:guid}", randomId.ToString());
        
        // Act
        var response = await _httpClient.PutAsJsonAsync(request, updatedBuyer);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ShouldReturn204_WhenBuyerIsDeleted()
    {
        // Arrange
        var buyerToDelete = _buyers.Last();
        string request = ApiEndpoints.Buyer.Delete.Replace("{id:guid}", buyerToDelete.Id.ToString());

        // Act
        var response = await _httpClient.DeleteAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Delete_ShouldReturn204_WhenBuyerIsNotDeleted()
    {
        // Arrange
        var randomId = Guid.NewGuid();
        string request = ApiEndpoints.Buyer.Delete.Replace("{id:guid}", randomId.ToString());

        // Act
        var response = await _httpClient.DeleteAsync(request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}