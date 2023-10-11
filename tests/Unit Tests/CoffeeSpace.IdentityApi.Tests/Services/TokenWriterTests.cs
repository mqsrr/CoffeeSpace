using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoFixture;
using CoffeeSpace.Core.Settings;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.IdentityApi.Tests.Services;

public class TokenWriterTests
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly IFixture _fixture; // Using AutoFixture for test data generation
    
    private readonly TokenWriter _tokenWriter;


    public TokenWriterTests()
    {
        _fixture = new Fixture();
        
        _userClaimsPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _jwtSettings = Options.Create(new JwtSettings
        {
            Key = "testKeysdl;fjsdlkf23",
            Issuer = "test",
            Audience = "test",
            Expire = 60
        });

        _tokenWriter = new TokenWriter(_jwtSettings, _userClaimsPrincipalFactory);
    }

    private string WriteToken(ClaimsPrincipal claimsPrincipal)
    {
        var handler = new JwtSecurityTokenHandler();
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
        var signingCred = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        var jwtSecurityToken = new JwtSecurityToken(
            _jwtSettings.Value.Issuer,
            _jwtSettings.Value.Audience,
            claimsPrincipal.Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_jwtSettings.Value.Expire),
            signingCred);

        return handler.WriteToken(jwtSecurityToken);
    }

    [Fact]
    public async Task WriteTokenAsync_ReturnsValidJwtToken()
    {
        // Arrange
        var user = _fixture.Create<ApplicationUser>();
        var mockClaimsPrincipal = Substitute.For<ClaimsPrincipal>();

        _userClaimsPrincipalFactory.CreateAsync(user).Returns(mockClaimsPrincipal);

        string expectedToken = WriteToken(mockClaimsPrincipal);

        // Act
        string token = await _tokenWriter.WriteTokenAsync(user, CancellationToken.None);
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var givenToken= handler.ReadJwtToken(expectedToken);

        // Assert
        jwtToken.Claims.Should().BeEquivalentTo(givenToken.Claims, options => options.Excluding(claim => claim.Value));
        jwtToken.Header.Alg.Should().Be(SecurityAlgorithms.HmacSha256Signature);
    }
}