using System.Globalization;
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

public sealed class ProductsApiControllerTests : IClassFixture<ProductApiFactory>
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
        Recording.Start("all");

        const string uriRequest = ApiEndpoints.Products.GetAll;
        
        // Act
        var response = await _httpClient.GetAsync(uriRequest);

        // Assert
        var sqlLogs = Recording.Stop("all");

        await Verify(new
        {
            response, 
            sqlLogs
        }).IgnoreMember("Authorization");
    }
    
    [Fact]
    public async Task GetById_ShouldReturn200_AndExistingProduct()
    {
        // Arrange
        Recording.Start("get");

        var expectedProductResponse = _products.First().ToResponse();
        string uriRequest = ApiEndpoints.Products.Get.Replace("{id:guid}", expectedProductResponse.Id.ToString());
                
        // Act
        var response = await _httpClient.GetAsync(uriRequest);

        // Assert
        var productResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();
        var sqlLogs =Recording.Stop("get");

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
        Recording.Start("create");

        var updatedProduct = AutoFaker.Generate<CreateProductRequest, CreateProductRequestFaker>();
        const string uriRequest = ApiEndpoints.Products.Create;
                
        var content = new MultipartFormDataContent();

        // Add string fields
        content.Add(new StringContent(updatedProduct.Id.ToString()), "Id");
        content.Add(new StringContent(updatedProduct.Title), "Title");
        content.Add(new StringContent(updatedProduct.Description), "Description");
        content.Add(new StringContent(updatedProduct.UnitPrice.ToString(CultureInfo.InvariantCulture)), "UnitPrice");
        content.Add(new StringContent(updatedProduct.Quantity.ToString()), "Quantity");

        // Add file part
        var fileContent = new StreamContent(updatedProduct.Image.OpenReadStream());
        content.Add(fileContent, "Image", updatedProduct.Image.FileName);
        
        // Act
        var response = await _httpClient.PostAsync(uriRequest, content);

        // Assert
        var sqlLogs =Recording.Stop("create");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Authorization", "Location", "Content-Type");
    }
    
    [Fact]
    public async Task Update_ShouldReturn200_AndUpdateProduct()
    {
        // Arrange
        Recording.Start("update");

        var productToUpdate = _products.First();
        var updatedProduct = AutoFaker.Generate<UpdateProductRequest, UpdateProductRequestFaker>();
        string uriRequest = ApiEndpoints.Products.Update.Replace("{id:guid}", productToUpdate.Id.ToString());
                
        var content = new MultipartFormDataContent();

        // Add string fields
        content.Add(new StringContent(updatedProduct.Id.ToString()), "Id");
        content.Add(new StringContent(updatedProduct.Title), "Title");
        content.Add(new StringContent(updatedProduct.Description), "Description");
        content.Add(new StringContent(updatedProduct.UnitPrice.ToString(CultureInfo.InvariantCulture)), "UnitPrice");
        content.Add(new StringContent(updatedProduct.Quantity.ToString()), "Quantity");

        // Add file part
        var fileContent = new StreamContent(updatedProduct.Image.OpenReadStream());
        content.Add(fileContent, "Image", updatedProduct.Image.FileName);

        // Act
        var response = await _httpClient.PutAsync(uriRequest, content);

        // Assert
        var sqlLogs =Recording.Stop("update");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Authorization", "Content-Type");
    }
    
    [Fact]
    public async Task Delete_ShouldReturn200_AndDeleteProduct()
    {
        // Arrange
        Recording.Start("delete");

        var productToDelete = _products.Last();
        string uriRequest = ApiEndpoints.Products.Delete.Replace("{id:guid}", productToDelete.Id.ToString());

        // Act
        var response = await _httpClient.DeleteAsync(uriRequest);

        // Assert
        var sqlLogs =Recording.Stop("delete");
        await Verify(new
        {
            response,
            sqlLogs
        }).IgnoreMembers("Authorization", "RequestUri");
    }
}