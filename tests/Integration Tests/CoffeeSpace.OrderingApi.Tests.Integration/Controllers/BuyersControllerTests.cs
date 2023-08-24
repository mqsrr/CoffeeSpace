using System.Net.Http.Json;
using System.Text.Json;
using AutoBogus;
using Bogus.Extensions;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Buyers;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Requests;
using CoffeeSpace.OrderingApi.Tests.Integration.Fixtures;
using Newtonsoft.Json;
using VerifyTests.EntityFramework;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Controllers;

[UsesVerify]
[Collection("Ordering Dependencies")]
public sealed class BuyersControllerTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly IEnumerable<Buyer> _buyers;

    public BuyersControllerTests(OrderingApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _buyers = apiFactory.Buyers;
    }

    [Fact]
    public async Task GetById_ShouldReturn200_WithExistingBuyer()
    {
        // Arrange
        var buyer = _buyers.First();
        string request = ApiEndpoints.Buyer.Get.Replace("{id:guid}", buyer.Id);

        // Act
        var response = await _httpClient.GetAsync(request);

        // Assert
        var sqlLogs = EfRecording.FinishRecording("OrderingDb");
        var buyerResponse = await response.Content.ReadFromJsonAsync<BuyerResponse>();

        await Verify(new
        {
            response,
            buyerResponse,
            sqlLogs
        }).IgnoreMember("Authorization");
    }
    
    [Fact]
    public async Task GetByEmail_ShouldReturn200_WithExistingBuyer()
    {
        // Arrange
        var buyer = _buyers.First();
        string request = ApiEndpoints.Buyer.GetWithEmail.Replace("{email}", buyer.Email);

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
    public async Task Create_ShouldReturn201_AndCreateBuyer()
    {
        // Arrange
        const string request = ApiEndpoints.Buyer.Create;
        var buyerToCreate = AutoFaker.Generate<CreateBuyerRequest, CreateBuyerRequestFaker>();

        // Act
        var response = await _httpClient.PostAsJsonAsync(request, buyerToCreate);

        // Assert
        var sqlLogs = EfRecording.FinishRecording("OrderingDb");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Authorization", "Location");
    }

    [Fact]
    public async Task Update_ShouldReturn200_AndUpdateBuyer()
    {
        // Arrange
        var buyer = _buyers.ElementAt(1);
        var buyerToUpdate = AutoFaker.Generate<Buyer, BuyerFaker>(3).Last();
        
        string request = ApiEndpoints.Buyer.Update.Replace("{id:guid}", buyer.Id);
        
        // Act
        var response = await _httpClient.PutAsJsonAsync(request, buyerToUpdate);

        // Assert
        var sqlLogs = EfRecording.FinishRecording("OrderingDb");
        var buyerResponse = response.Content.ReadFromJsonAsync<BuyerResponse>();
        
        await Verify(new
        {
            response,
            buyerResponse,    
            sqlLogs
        }).IgnoreMember("Authorization");
    }

    [Fact]
    public async Task Delete_ShouldReturn200_DeleteBuyer()
    {
        // Arrange
        var buyerToDelete = _buyers.Last();
        string request = ApiEndpoints.Buyer.Delete.Replace("{id:guid}", buyerToDelete.Id);

        // Act
        var response = await _httpClient.DeleteAsync(request);

        // Assert
        var sqlLogs = EfRecording.FinishRecording("OrderingDb");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMember("Authorization");
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