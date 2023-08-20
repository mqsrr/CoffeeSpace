using System.Security.Claims;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.IdentityApi.Tests.Services;

public sealed class AuthServiceTests
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenWriter<ApplicationUser> _tokenWriter;
    private readonly ISendEndpointProvider _endpointProvider;
    private readonly Fixture _fixture;

    private readonly AuthService _authService;
    
    public AuthServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _userManager = GetUserManagerMock();
        _signInManager = GetSignInManagerMock(_userManager);

        _tokenWriter = _fixture.Create<ITokenWriter<ApplicationUser>>();
        _endpointProvider = _fixture.Create<ISendEndpointProvider>();

        _authService = new AuthService(_signInManager, _tokenWriter, _endpointProvider);
    }
    
    private static SignInManager<ApplicationUser> GetSignInManagerMock(UserManager<ApplicationUser> userManager)
    {
        var httpContext = new DefaultHttpContext
        {
            Request =
            {
                Scheme = "http",
                Host = new HostString("localhost", 5000)
            }
        };

        var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
        mockHttpContextAccessor.HttpContext
            .Returns(httpContext);

        var mockClaimsFactory = Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>();
        mockClaimsFactory.CreateAsync(Arg.Any<ApplicationUser>())
            .Returns(callInfo =>
            {
                var user = callInfo.Arg<ApplicationUser>();
                return new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email!)
                        }));
            });

        var mockSignInManager = Substitute.For<SignInManager<ApplicationUser>>(userManager, mockHttpContextAccessor,
            mockClaimsFactory, null, null, null, null);

        return mockSignInManager;
    }

    private static UserManager<ApplicationUser> GetUserManagerMock()
    {
        var mockUserStore = Substitute.For<IUserStore<ApplicationUser>>();
        var mockUserManager =
            Substitute.For<UserManager<ApplicationUser>>(mockUserStore, null, null, null, null, null, null, null, null);

        return mockUserManager;
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldReturnToken_WhenRegisterSucceeds()
    {
        // Arrange
        var user = _fixture.Create<ApplicationUser>();
        string expectedToken = _fixture.Create<string>();
        
        _userManager.CreateAsync(user, user.Password)
            .Returns(IdentityResult.Success);

        _signInManager.PasswordSignInAsync(user.UserName!, user.Password, false, false)
            .Returns(SignInResult.Success);

        _signInManager.CreateUserPrincipalAsync(user)
            .Returns(_fixture.Create<ClaimsPrincipal>());

        _tokenWriter.WriteTokenAsync(user, Arg.Any<CancellationToken>())
            .Returns(expectedToken);

        // Act
        string? jwtToken = await _authService.RegisterAsync(user, CancellationToken.None);

        // Assert     
        jwtToken.Should().BeEquivalentTo(expectedToken);
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldReturnNull_WhenRegisterFailsOnPasswordSignIn()
    {
        // Arrange
        var user = _fixture.Create<ApplicationUser>();
        
        _userManager.CreateAsync(user, user.Password)
            .Returns(IdentityResult.Success);

        _signInManager.PasswordSignInAsync(user.UserName!, user.Password, false, false)
            .Returns(SignInResult.Failed);
        
        // Act
        string? jwtToken = await _authService.RegisterAsync(user, CancellationToken.None);

        // Assert     
        jwtToken.Should().BeNull();
        await _endpointProvider.DidNotReceive().GetSendEndpoint(Arg.Any<Uri>());
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldReturnNull_WhenRegisterFailsOnUserCreation()
    {
        // Arrange
        var user = _fixture.Create<ApplicationUser>();
        
        _userManager.CreateAsync(user, user.Password)
            .Returns(IdentityResult.Failed());
        
        // Act
        string? jwtToken = await _authService.RegisterAsync(user, CancellationToken.None);

        // Assert     
        jwtToken.Should().BeNull();
        await _endpointProvider.DidNotReceive().GetSendEndpoint(Arg.Any<Uri>());
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenLoginSucceeds()
    {
        // Arrange
        var user = _fixture.Create<ApplicationUser>();
        string expectedToken = _fixture.Create<string>();
        
        _signInManager.PasswordSignInAsync(user.UserName!, user.Password, false, false)
            .Returns(SignInResult.Success);

        _userManager.FindByNameAsync(user.UserName!)
            .Returns(user);

        _tokenWriter.WriteTokenAsync(user, Arg.Any<CancellationToken>())
            .Returns(expectedToken);
        
        // Act
        string? token = await _authService.LoginAsync(user, CancellationToken.None);

        // Assert
        token.Should().BeEquivalentTo(expectedToken);
    }
    
    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenLoginFailsOnSignIn()
    {
        // Arrange
        var user = _fixture.Create<ApplicationUser>();
        
        _signInManager.PasswordSignInAsync(user.UserName!, user.Password, false, false)
            .Returns(SignInResult.Failed);
        
        // Act
        string? token = await _authService.LoginAsync(user, CancellationToken.None);

        // Assert
        token.Should().BeNull();
    }
}