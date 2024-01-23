using System.Net.Http.Json;
using AutoBogus;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using CoffeeSpace.ProductApi.Application.Helpers;
using CoffeeSpace.ProductApi.Application.Mapping;
using CoffeeSpace.ProductApi.Tests.Integration.Fakers;
using CoffeeSpace.ProductApi.Tests.Integration.Fixtures;

namespace CoffeeSpace.ProductApi.Tests.Integration.Controllers;

[UsesVerify]
public sealed class ProductsApiControllerTests : IClassFixture<ProductApiFactory>, IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly IEnumerable<Product> _products;
    
    public ProductsApiControllerTests(ProductApiFactory apiFactory)
    {
        _httpClient = apiFactory.CreateClient();
        _products = apiFactory.Products;
    }

    [Fact]
    public async Task GetAll_ShouldReturn200_AndAllProducts()
    {
        // Arrange
        const string uriRequest = ApiEndpoints.Products.GetAll;
        
        // Act
        var response = await _httpClient.GetAsync(uriRequest);

        // Assert
        var productResponses = await response.Content.ReadFromJsonAsync<IEnumerable<ProductResponse>>();
        var sqlLogs = Recording.Stop("ProductDb");

        await Verify(new
        {
            response, 
            productResponses,
            sqlLogs
        }).IgnoreMember("Authorization");
    }
    
    [Fact]
    public async Task GetById_ShouldReturn200_AndExistingProduct()
    {
        // Arrange
        var expectedProductResponse = _products.First().ToResponse();
        string uriRequest = ApiEndpoints.Products.Get.Replace("{id:guid}", expectedProductResponse.Id);
                
        // Act
        var response = await _httpClient.GetAsync(uriRequest);

        // Assert
        var productResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();
        var sqlLogs =Recording.Stop("ProductDb");

        await Verify(new
        {
            response,
            productResponse,
            sqlLogs
        }).IgnoreMember("Authorization");
    }
    
    [Fact]
    public async Task Create_ShouldReturn201_AndCreateProduct()
    {
        // Arrange
        var productToCreate = AutoFaker.Generate<CreateProductRequest, CreateProductRequestFaker>();
        const string uriRequest = ApiEndpoints.Products.Create;
                
        // Act
        var response = await _httpClient.PostAsJsonAsync(uriRequest, productToCreate);

        // Assert
        var sqlLogs =Recording.Stop("ProductDb");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Authorization", "Location");
    }
    
    [Fact]
    public async Task Update_ShouldReturn200_AndUpdateProduct()
    {
        // Arrange
        var productToUpdate = _products.First();
        var updatedProduct = AutoFaker.Generate<UpdateProductRequest, UpdateProductRequestFaker>();
        string uriRequest = ApiEndpoints.Products.Update.Replace("{id:guid}", productToUpdate.Id);
                
        // Act
        var response = await _httpClient.PutAsJsonAsync(uriRequest, updatedProduct);

        // Assert
        var sqlLogs =Recording.Stop("ProductDb");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMember("Authorization");
    }
    
    [Fact]
    public async Task Delete_ShouldReturn200_AndDeleteProduct()
    {
        // Arrange
        var productToDelete = _products.Last();
        string uriRequest = ApiEndpoints.Products.Delete.Replace("{id:guid}", productToDelete.Id);

        // Act
        var response = await _httpClient.DeleteAsync(uriRequest);

        // Assert
        var sqlLogs =Recording.Stop("ProductDb");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Authorization", "RequestUri");
    }

    public Task InitializeAsync()
    {
        Recording.Start("ProductDb");
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}