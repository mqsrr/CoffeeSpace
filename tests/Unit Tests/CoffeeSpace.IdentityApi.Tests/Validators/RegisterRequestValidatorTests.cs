using AutoFixture;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using CoffeeSpace.IdentityApi.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.IdentityApi.Tests.Validators;

public sealed class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _registerRequestValidator;
    private readonly Fixture _fixture;

    public RegisterRequestValidatorTests()
    {
        _registerRequestValidator = new RegisterRequestValidator();
        _fixture = new Fixture();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidName_ShouldThrowValidationError(string name)
    {
        // Arrange
        var request = _fixture.Build<RegisterRequest>()
            .With(registerRequest => registerRequest.Name, name)
            .Create();

        // Act
        var result = await _registerRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(registerRequest => registerRequest.Name);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidLastName_ShouldThrowValidationError(string lastName)
    {
        // Arrange
        var request = _fixture.Build<RegisterRequest>()
            .With(registerRequest => registerRequest.LastName, lastName)
            .Create();

        // Act
        var result = await _registerRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(registerRequest => registerRequest.LastName);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidUserName_ShouldThrowValidationError(string username)
    {
        // Arrange
        var request = _fixture.Build<RegisterRequest>()
            .With(registerRequest => registerRequest.UserName, username)
            .Create();

        // Act
        var result = await _registerRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(registerRequest => registerRequest.UserName);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("hellogmaill.com")]
    public async Task InvalidEmail_ShouldThrowValidationError(string email)
    {
        // Arrange
        var request = _fixture.Build<RegisterRequest>()
            .With(registerRequest => registerRequest.Email, email)
            .Create();

        // Act
        var result = await _registerRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(registerRequest => registerRequest.Email);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("spas")]
    [InlineData("notHaveNumbers")]
    [InlineData("notHaveSymbols222")]
    public async Task InvalidPassword_ShouldThrowValidationError(string password)
    {
        // Arrange
        var request = _fixture.Build<RegisterRequest>()
            .With(registerRequest => registerRequest.Password, password)
            .Create();

        // Act
        var result = await _registerRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(registerRequest => registerRequest.Password);
    }
    
    [Theory]
    [InlineData("name", "lastName", "username", "passworD11!", "any@gmail.com")]
    public async Task ValidProperties_ShouldNotThrowAnyValidationErrors(string name, string lastName, string username, string password, string email)
    {
        // Arrange
        var request = _fixture.Build<RegisterRequest>()
            .With(registerRequest => registerRequest.Name, name)
            .With(registerRequest => registerRequest.LastName, lastName)
            .With(registerRequest => registerRequest.UserName, username)
            .With(registerRequest => registerRequest.Password, password)
            .With(registerRequest => registerRequest.Email, email)
            .Create();

        // Act
        var result = await _registerRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}