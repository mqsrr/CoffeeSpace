using AutoFixture;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;
using CoffeeSpace.OrderingApi.Application.Validators.Buyers;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Validators.Buyers;

public sealed class UpdateBuyerRequestValidatorTests
{
    private readonly UpdateBuyerRequestValidator _updateBuyerRequestValidator;
    private readonly Fixture _fixture;

    public UpdateBuyerRequestValidatorTests()
    {
        _fixture = new Fixture();
        _updateBuyerRequestValidator = new UpdateBuyerRequestValidator();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidName_ShouldThrowValidationError(string name)
    {
        // Arrange
        var request = _fixture.Build<UpdateBuyerRequest>()
            .With(buyerRequest => buyerRequest.Name, name)
            .Create();

        // Act
        var result = await _updateBuyerRequestValidator.TestValidateAsync(request);

        // Assert   
        result.ShouldHaveValidationErrorFor(buyerRequest => buyerRequest.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("incorrectgmail.com")]
    public async Task InvalidEmail_ShouldThrowValidationError(string email)
    {
        // Arrange
        var request = _fixture.Build<UpdateBuyerRequest>()
            .With(buyerRequest => buyerRequest.Email, email)
            .Create();

        // Act
        var result = await _updateBuyerRequestValidator.TestValidateAsync(request);

        // Assert   
        result.ShouldHaveValidationErrorFor(buyerRequest => buyerRequest.Email);
    }

    [Theory]
    [InlineData("name", "email@gmail.com")]
    [InlineData("test", "another@gmail.com")]
    public async Task ValidProperties_ShouldNotThrowAnyValidationErrors(string name, string email)
    {
        // Arrange
        var request = _fixture.Build<UpdateBuyerRequest>()
            .With(buyerRequest => buyerRequest.Name, name)
            .With(buyerRequest => buyerRequest.Email, email)
            .Create();

        // Act
        var result = await _updateBuyerRequestValidator.TestValidateAsync(request);

        // Assert   
        result.ShouldNotHaveAnyValidationErrors();
    }
}