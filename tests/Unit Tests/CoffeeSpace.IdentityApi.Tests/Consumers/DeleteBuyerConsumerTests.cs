using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.IdentityApi.Application.Messages.Consumers;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.Messages.Buyers;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.IdentityApi.Tests.Consumers;

public sealed class DeleteBuyerConsumerTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<DeleteBuyerConsumer> _consumerTestHarness;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Fixture _fixture;

    public DeleteBuyerConsumerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        var serviceProvider = new ServiceCollection()
            .AddScoped<UserManager<ApplicationUser>>(_ => _userManager)
            .AddMassTransitTestHarness(config => config.AddConsumer<DeleteBuyerConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<DeleteBuyerConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var userToDelete = _fixture.Create<ApplicationUser>();
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<DeleteBuyerConsumer>();
        string email = _fixture.Create<string>();

        _userManager.FindByEmailAsync(email)
            .Returns(userToDelete);
        
        // Act
        await consumerEndpoint.Send<DeleteBuyerByEmail>(new
        {
            Email = email
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<DeleteBuyerByEmail>();
        consumedAny.Should().BeTrue();

        await _userManager.Received().DeleteAsync(userToDelete);
    }
    
    [Fact]
    public async Task Consume_ShouldNotDeleteUser_WhenUserDoesNotExist()
    {
        // Arrange
        var userToDelete = _fixture.Create<ApplicationUser>();
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<DeleteBuyerConsumer>();
        string email = _fixture.Create<string>();

        _userManager.FindByEmailAsync(email)
            .ReturnsNull();
        
        // Act
        await consumerEndpoint.Send<DeleteBuyerByEmail>(new
        {
            Email = email
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<DeleteBuyerByEmail>();
        consumedAny.Should().BeTrue();

        await _userManager.DidNotReceive().DeleteAsync(userToDelete);
    }
    
    public async Task InitializeAsync()
    {
        await _testHarness.Start();
    }

    public async Task DisposeAsync()
    {
        await _testHarness.Stop();
    }
}