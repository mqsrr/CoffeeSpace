using System.Net;
using System.Net.Http.Json;
using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using CoffeeSpace.ProductApi.Tests.Acceptance.Helpers;
using CoffeeSpace.ProductApi.Tests.Acceptance.Models;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CoffeeSpace.ProductApi.Tests.Acceptance.StepDefinitions;

[Binding]
public sealed class ProductManagementSteps
{
    private readonly HttpClient _httpClient;
    private readonly ScenarioContext _scenarioContext;

    public ProductManagementSteps(HttpClient httpClient, ScenarioContext scenarioContext)
    {
        _httpClient = httpClient;
        _scenarioContext = scenarioContext;
    }

    [Given(@"the following products in the system:")]
    public async Task GivenTheFollowingProductsInTheSystem(Table table)
    {
        var createProductRequests = table.CreateSet<CreateProductRequest>();
        var createdProducts = new List<ProductResponse>();

        foreach (var createProductRequest in createProductRequests)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Products.Create, createProductRequest);
            var createdProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();

            createdProducts.Add(createdProduct!);
        }
        _scenarioContext.Add("Created Products", createdProducts);
    }

    [When(@"the POST request is sent with following request:")]
    public async Task WhenThePostRequestIsSentWithFollowingRequest(Table table)
    {
        var createProductRequest = table.CreateInstance<CreateProductRequest>();
        var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Products.Create, createProductRequest);
        
        var createdProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();
        _scenarioContext.Add("Created Product", createdProduct);
        _scenarioContext.Add("Response Status Code", response.StatusCode);
    }

    [Then(@"the product are created successfully")]
    public async Task ThenTheProductAreCreatedSuccessfully()
    {
        var createdProduct = _scenarioContext.Get<ProductResponse>("Created Product"); 
        string getProductEndpointUri = ApiEndpoints.Products.Get(createdProduct.Id);

        var productResponse = await _httpClient.GetFromJsonAsync<ProductResponse>(getProductEndpointUri);
        productResponse.Should().BeEquivalentTo(createdProduct);
    }

    [Then(@"the response status code should be (.*)")]
    public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
    {
        var actualStatusCode = _scenarioContext.Get<HttpStatusCode>("Response Status Code");
        actualStatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }

    [When(@"a GET request is sent to retrieve the product by ID")]
    public async Task WhenAgetRequestIsSentToRetrieveTheProductById()
    {
        var createdProducts = _scenarioContext.Get<List<ProductResponse>>("Created Products");
        var productToRetrieve = createdProducts.First();

        string getProductEndpointUri = ApiEndpoints.Products.Get(productToRetrieve.Id);
        var response = await _httpClient.GetAsync(getProductEndpointUri);
        var productResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();
        
        _scenarioContext.Add("Retrieved Product Response", productResponse);
        _scenarioContext.Add("Response Status Code", response.StatusCode);
    }

    [Then(@"the response body should contain the correct product information")]
    public void ThenTheResponseBodyShouldContainTheCorrectProductInformation()
    {
        var response = _scenarioContext.Get<ProductResponse>("Retrieved Product Response");
        var productToRetrieve = _scenarioContext.Get<List<ProductResponse>>("Created Products").First();
        
        response.Should().BeEquivalentTo(productToRetrieve);
    }

    [When(@"a PUT request is sent to update the product with updated product data:")]
    public async Task WhenAputRequestIsSentToUpdateTheProductWithUpdatedProductData(Table table)
    {
        var updateProductRequest = table.CreateInstance<UpdateProductRequest>();
        var existingProduct = _scenarioContext.Get<List<ProductResponse>>("Created Products").First();
        string updateProductEndpointUri = ApiEndpoints.Products.Update(existingProduct.Id);

        var response = await _httpClient.PutAsJsonAsync(updateProductEndpointUri, updateProductRequest);
        var updatedProductResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();
        
        _scenarioContext.Add("Updated Product Response", updatedProductResponse);
        _scenarioContext.Add("Response Status Code", response.StatusCode);
    }
    
    [Then(@"a GET request to retrieve the updated product should return the correct updated product information with (.*) status code")]
    public async Task WhenAgetRequestToRetrieveTheUpdatedProductShouldReturnTheCorrectUpdatedProductInformationWithStatusCode(int expectedStatusCode)
    {
        var updatedProduct = _scenarioContext.Get<ProductResponse>("Updated Product Response");
        string getProductEndpointUri = ApiEndpoints.Products.Get(updatedProduct.Id);

        var response = await _httpClient.GetAsync(getProductEndpointUri);
        var productResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();
        
        response.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
        productResponse.Should().BeEquivalentTo(updatedProduct);
    }

    [When(@"a DELETE request is sent to delete the product by ID")]
    public async Task WhenAdeleteRequestIsSentToDeleteTheProductById()
    {
        var existingProduct = _scenarioContext.Get<List<ProductResponse>>("Created Products").First();
        string deleteProductEndpointUri = ApiEndpoints.Products.Delete(existingProduct.Id);

        var response = await _httpClient.DeleteAsync(deleteProductEndpointUri);
        _scenarioContext.Add("Response Status Code", response.StatusCode);
        _scenarioContext.Add("Deleted Product's Id", existingProduct.Id);
    }

    [Then(@"a GET request to retrieve the deleted product should return a status code of (.*) \(Not Found\)")]
    public async Task ThenAgetRequestToRetrieveTheDeletedProductShouldReturnAStatusCodeOfNotFound(int expectedStatusCode)
    {
        string deletedProductId = _scenarioContext.Get<string>("Deleted Product's Id");
        string getProductEndpointUri = ApiEndpoints.Products.Get(deletedProductId);

        var response = await _httpClient.GetAsync(getProductEndpointUri);
        response.StatusCode.Should().Be((HttpStatusCode)expectedStatusCode);
    }
}