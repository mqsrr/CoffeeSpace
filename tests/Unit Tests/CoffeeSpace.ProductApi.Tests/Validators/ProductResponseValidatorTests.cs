using AutoFixture;
using CoffeeSpace.ProductApi.Application.Contracts.Responses;
using CoffeeSpace.ProductApi.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.ProductApi.Tests.Validators;

public sealed class ProductResponseValidatorTests
{
    private readonly ProductResponseValidator _productResponseValidator;
    private readonly Fixture _fixture;

    public ProductResponseValidatorTests()
    {
        _productResponseValidator = new ProductResponseValidator();
        _fixture = new Fixture();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidTitle_ShouldThrowValidationError(string title)
    {
        // Arrange
        var request = _fixture.Build<ProductResponse>()
            .With(productRequest => productRequest.Title, title)
            .Create();
        
        // Act
        var result = await _productResponseValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.Title);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidDescription_ShouldThrowValidationError(string description)
    {
        // Arrange
        var request = _fixture.Build<ProductResponse>()
            .With(productRequest => productRequest.Description, description)
            .Create();
        
        // Act
        var result = await _productResponseValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.Description);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    [InlineData(-1)]
    public async Task InvalidPrice_ShouldThrowValidationError(int price)
    {
        // Arrange
        var request = _fixture.Build<ProductResponse>()
            .With(productRequest => productRequest.UnitPrice, price)
            .Create();
        
        // Act
        var result = await _productResponseValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.UnitPrice);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidQuantity_ShouldThrowValidationError(int quantity)
    {
        // Arrange
        var request = _fixture.Build<ProductResponse>()
            .With(productRequest => productRequest.Quantity, quantity)
            .Create();
        
        // Act
        var result = await _productResponseValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.Quantity);
    }
    
    [Fact]
    public async Task ValidProperties_ShouldNotThrowAnyValidationErrors()
    {
        // Arrange
        var request = _fixture.Build<ProductResponse>()
            .With(productRequest => productRequest.UnitPrice, Random.Shared.Next(1, 99))
            .With(productRequest => productRequest.Quantity, Random.Shared.Next(1, 10))
            .Create();
        
        // Act
        var result = await _productResponseValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}