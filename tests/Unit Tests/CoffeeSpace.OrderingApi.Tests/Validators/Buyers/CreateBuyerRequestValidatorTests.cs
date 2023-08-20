using AutoFixture;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;
using CoffeeSpace.OrderingApi.Application.Validators.Buyers;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Validators.Buyers;

public sealed class CreateBuyerRequestValidatorTests
{
    private readonly CreateBuyerRequestValidator _createBuyerRequestValidator;
    private readonly Fixture _fixture;

    public CreateBuyerRequestValidatorTests()
    {
        _fixture = new Fixture();
        _createBuyerRequestValidator = new CreateBuyerRequestValidator();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidName_ShouldThrowValidationError(string name)
    {
        // Arrange
        var request = _fixture.Build<CreateBuyerRequest>()
            .With(request => request.Name, name)
            .Create();

        // Act
        var result = await _createBuyerRequestValidator.TestValidateAsync(request);

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
        var request = _fixture.Build<CreateBuyerRequest>()
            .With(request => request.Email, email)
            .Create();

        // Act
        var result = await _createBuyerRequestValidator.TestValidateAsync(request);

        // Assert   
        result.ShouldHaveValidationErrorFor(buyerRequest => buyerRequest.Email);
    }
    
    [Theory]
    [InlineData("name", "some@gmail.com")]
    [InlineData("test", "test.3434@gmail.com")]
    public async Task ValidProperties_ShouldNotThrowAnyValidationErrors(string name, string email)
    {
        // Arrange
        var request = _fixture.Build<CreateBuyerRequest>()
            .With(request => request.Name, name)
            .With(request => request.Email, email)
            .Create();

        // Act
        var result = await _createBuyerRequestValidator.TestValidateAsync(request);

        // Assert   
        result.ShouldNotHaveAnyValidationErrors();
    }
}