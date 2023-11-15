using AutoFixture;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.ProductApi.Tests.Validators;

public sealed class CreateProductRequestValidatorTests
{
    private readonly CreateProductRequestValidator _createProductRequestValidator;
    private readonly Fixture _fixture;
    
    public CreateProductRequestValidatorTests()
    {
        _createProductRequestValidator = new CreateProductRequestValidator();
        _fixture = new Fixture();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidTitle_ShouldThrowValidationError(string title)
    {
        // Arrange
        var request = _fixture.Build<CreateProductRequest>()
            .With(productRequest => productRequest.Title, title)
            .Create();
        
        // Act
        var result = await _createProductRequestValidator.TestValidateAsync(request);
        
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
        var request = _fixture.Build<CreateProductRequest>()
            .With(productRequest => productRequest.Description, description)
            .Create();
        
        // Act
        var result = await _createProductRequestValidator.TestValidateAsync(request);
        
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
        var request = _fixture.Build<CreateProductRequest>()
            .With(productRequest => productRequest.UnitPrice, price)
            .Create();
        
        // Act
        var result = await _createProductRequestValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.UnitPrice);
    }
    
    [Theory]
    [InlineData(0f)]
    [InlineData(100f)]
    [InlineData(-1f)]
    public async Task InvalidDiscount_ShouldThrowValidationError(float discount)
    {
        // Arrange
        var request = _fixture.Build<CreateProductRequest>()
            .With(productRequest => productRequest.Discount, discount)
            .Create();
        
        // Act
        var result = await _createProductRequestValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.Discount);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidQuantity_ShouldThrowValidationError(int quantity)
    {
        // Arrange
        var request = _fixture.Build<CreateProductRequest>()
            .With(productRequest => productRequest.Quantity, quantity)
            .Create();
        
        // Act
        var result = await _createProductRequestValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.Quantity);
    }
    
    [Fact]
    public async Task ValidProperties_ShouldNotThrowAnyValidationErrors()
    {
        // Arrange
        var request = _fixture.Build<CreateProductRequest>()
            .With(productRequest => productRequest.UnitPrice, Random.Shared.Next(1, 99))
            .With(productRequest => productRequest.Discount, Random.Shared.NextDouble())
            .With(productRequest => productRequest.Quantity, Random.Shared.Next(1, 10))
            .Create();
        
        // Act
        var result = await _createProductRequestValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}