using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.IdentityApi.Application.Messages.Consumers;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.Messages.Buyers;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.IdentityApi.Tests.Consumers;

public sealed class UpdateBuyerConsumerTests : IAsyncLifetime
{
    private readonly ITestHarness _testHarness;
    private readonly IConsumerTestHarness<UpdateBuyerConsumer> _consumerTestHarness;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Fixture _fixture;

    public UpdateBuyerConsumerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        var serviceProvider = new ServiceCollection()
            .AddScoped<UserManager<ApplicationUser>>(_ => _userManager)
            .AddMassTransitTestHarness(config => config.AddConsumer<UpdateBuyerConsumer>())
            .BuildServiceProvider(true);

        _testHarness = serviceProvider.GetTestHarness();
        _consumerTestHarness = _testHarness.GetConsumerHarness<UpdateBuyerConsumer>();
    }

    [Fact]
    public async Task Consume_ShouldConsumeMessages_AndUpdateBuyer()
    {
        // Arrange
        var userToUpdate = _fixture.Create<ApplicationUser>();
        var consumerEndpoint = await _testHarness.GetConsumerEndpoint<UpdateBuyerConsumer>();

        var buyer = _fixture.Create<Buyer>();
        _userManager.FindByEmailAsync(buyer.Email)
            .Returns(userToUpdate);
        
        // Act
        await consumerEndpoint.Send<UpdateBuyer>(new
        {
            Buyer = buyer
        });

        // Assert
        bool consumedAny = await _consumerTestHarness.Consumed.Any<UpdateBuyer>();
        consumedAny.Should().BeTrue();

        await _userManager.Received().SetEmailAsync(userToUpdate, buyer.Email);
        await _userManager.Received().SetUserNameAsync(userToUpdate, buyer.Name);
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