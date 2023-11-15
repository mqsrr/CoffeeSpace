using AutoFixture;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Buyers;
using CoffeeSpace.OrderingApi.Application.Validators.Buyers;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Validators.Buyers;

public sealed class BuyerResponseValidatorTests
{
    private readonly BuyerResponseValidator _responseValidator;
    private readonly Fixture _fixture;

    public BuyerResponseValidatorTests()
    {
        _fixture = new Fixture();
        _responseValidator = new BuyerResponseValidator();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidId_ShouldThrowValidationError(string Id)
    {
        // Arrange
        var response = _fixture.Build<BuyerResponse>()
            .With(buyerResponse => buyerResponse.Id, Id)
            .Create();

        // Act
        var result = await _responseValidator.TestValidateAsync(response);

        // Assert   
        result.ShouldHaveValidationErrorFor(buyerResponse => buyerResponse.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidName_ShouldThrowValidationError(string name)
    {
        // Arrange
        var response = _fixture.Build<BuyerResponse>()
            .With(buyerResponse => buyerResponse.Name, name)
            .Create();

        // Act
        var result = await _responseValidator.TestValidateAsync(response);

        // Assert   
        result.ShouldHaveValidationErrorFor(buyerResponse => buyerResponse.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("incorrectgmail.com")]
    public async Task InvalidEmail_ShouldThrowValidationError(string email)
    {
        // Arrange
        var response = _fixture.Build<BuyerResponse>()
            .With(buyerResponse => buyerResponse.Email, email)
            .Create();

        // Act
        var result = await _responseValidator.TestValidateAsync(response);

        // Assert   
        result.ShouldHaveValidationErrorFor(buyerResponse => buyerResponse.Email);
    }
    
    [Theory]
    [InlineData("name", "some@gmail.com")]
    [InlineData("test", "test.3434@gmail.com")]
    public async Task ValidProperties_ShouldNotThrowAnyValidationErrors(string name, string email)
    {
        // Arrange
        var response = _fixture.Build<BuyerResponse>()
            .With(buyerResponse => buyerResponse.Name, name)
            .With(buyerResponse => buyerResponse.Email, email)
            .Create();

        // Act
        var result = await _responseValidator.TestValidateAsync(response);

        // Assert   
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    
}