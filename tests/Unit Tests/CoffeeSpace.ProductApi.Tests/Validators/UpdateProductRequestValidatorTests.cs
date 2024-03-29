using AutoFixture;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.ProductApi.Tests.Validators;

public sealed class UpdateProductRequestValidatorTests
{
    private readonly UpdateProductRequestValidator _updateProductRequestValidator;
    private readonly Fixture _fixture;

    public UpdateProductRequestValidatorTests()
    {
        _updateProductRequestValidator = new UpdateProductRequestValidator();
        _fixture = new Fixture();
    }
    
        [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidTitle_ShouldThrowValidationError(string title)
    {
        // Arrange
        var request = _fixture.Build<UpdateProductRequest>()
            .With(productRequest => productRequest.Title, title)
            .Without(productRequest => productRequest.Image)
            .Create();
        
        // Act
        var result = await _updateProductRequestValidator.TestValidateAsync(request);
        
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
        var request = _fixture.Build<UpdateProductRequest>()
            .With(productRequest => productRequest.Description, description)
            .Without(productRequest => productRequest.Image)
            .Create();
        
        // Act
        var result = await _updateProductRequestValidator.TestValidateAsync(request);
        
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
        var request = _fixture.Build<UpdateProductRequest>()
            .With(productRequest => productRequest.UnitPrice, price)
            .Without(productRequest => productRequest.Image)
            .Create();
        
        // Act
        var result = await _updateProductRequestValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.UnitPrice);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task InvalidQuantity_ShouldThrowValidationError(int quantity)
    {
        // Arrange
        var request = _fixture.Build<UpdateProductRequest>()
            .With(productRequest => productRequest.Quantity, quantity)
            .Without(productRequest => productRequest.Image)
            .Create();
        
        // Act
        var result = await _updateProductRequestValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldHaveValidationErrorFor(productRequest => productRequest.Quantity);
    }
    
    [Fact]
    public async Task ValidProperties_ShouldNotThrowAnyValidationErrors()
    {
        // Arrange
        var request = _fixture.Build<UpdateProductRequest>()
            .With(productRequest => productRequest.UnitPrice, Random.Shared.Next(1, 99))
            .With(productRequest => productRequest.Quantity, Random.Shared.Next(1, 10))
            .Without(productRequest => productRequest.Image)
            .Create();
        
        // Act
        var result = await _updateProductRequestValidator.TestValidateAsync(request);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(productRequest => productRequest.UnitPrice);
        result.ShouldNotHaveValidationErrorFor(productRequest => productRequest.Quantity);
        result.ShouldNotHaveValidationErrorFor(productRequest => productRequest.Title);
        result.ShouldNotHaveValidationErrorFor(productRequest => productRequest.Description);
        result.ShouldNotHaveValidationErrorFor(productRequest => productRequest.Id);

        result.ShouldHaveValidationErrorFor(productRequest => productRequest.Image);
    }
}