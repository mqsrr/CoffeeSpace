using AutoFixture;
using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Login;
using CoffeeSpace.IdentityApi.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.IdentityApi.Tests.Validators;

public sealed class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _loginRequestValidator;
    private readonly Fixture _fixture;

    public LoginRequestValidatorTests()
    {
        _loginRequestValidator = new LoginRequestValidator();
        _fixture = new Fixture();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidUsername_ShouldThrowValidationError(string username)
    {
        // Arrange
        var request = _fixture.Build<LoginRequest>()
            .With(loginRequest => loginRequest.Username, username)
            .Create();

        // Act
        var result = await _loginRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(loginRequest => loginRequest.Username);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task InvalidPassword_ShouldThrowValidationError(string password)
    {
        // Arrange
        var request = _fixture.Build<LoginRequest>()
            .With(loginRequest => loginRequest.Password, password)
            .Create();

        // Act
        var result = await _loginRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(loginRequest => loginRequest.Password);
    }
    
    [Theory]
    [InlineData("username", "newPaass11!")]
    [InlineData("John", "Helloddd123!")]
    public async Task ValidProperties_ShouldNotThrowAnyValidationErrors(string username, string password)
    {
        // Arrange
        var request = _fixture.Build<LoginRequest>()
            .With(loginRequest => loginRequest.Username, username)
            .With(loginRequest => loginRequest.Password, password)
            .Create();

        // Act
        var result = await _loginRequestValidator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}