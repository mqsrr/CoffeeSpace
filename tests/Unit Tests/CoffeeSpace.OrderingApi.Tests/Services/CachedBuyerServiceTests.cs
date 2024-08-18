using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Notifications.Buyers;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Decorators;
using CoffeeSpace.Shared.Services.Abstractions;
using FluentAssertions;
using Mediator;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Services;

public sealed class CachedBuyerServiceTests
{
    private readonly ICacheService _cacheService;
    private readonly IBuyerService _buyerService;
    private readonly IPublisher _publisher;

    private readonly Fixture _fixture;
    private readonly CachedBuyerService _cachedBuyerService;

    public CachedBuyerServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        
        _cacheService = _fixture.Create<ICacheService>();
        _buyerService = _fixture.Create<IBuyerService>();
        _publisher = _fixture.Create<IPublisher>();

        _cachedBuyerService = new CachedBuyerService(_cacheService, _buyerService, _publisher);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCachedBuyer_WhenCachedBuyerExists()
    {
        // Arrange
        var buyer = _fixture.Create<Buyer>();
        var cancellationToken = CancellationToken.None;

        _cacheService.GetOrCreateAsync(CacheKeys.Buyers.Get(buyer.Id), Arg.Any<Func<Task<Buyer>>>()!, cancellationToken)
            .Returns(buyer);
        
        // Act
        var buyerResult = await _cachedBuyerService.GetByIdAsync(buyer.Id, cancellationToken);
        
        // Assert
        buyerResult.Should().NotBeNull();
        buyerResult.Should().BeEquivalentTo(buyer);

        await _buyerService.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBuyer_AndCreateCache_WhenCachedBuyerDoesNotExist()
    {
        // Arrange
        var buyer = _fixture.Create<Buyer>();
        var cancellationToken = CancellationToken.None;
        
        _cacheService.GetOrCreateAsync(CacheKeys.Buyers.Get(buyer.Id), Arg.Any<Func<Task<Buyer>>>()!, cancellationToken)!
            .Returns(info => info.Arg<Func<Task<Buyer>>>().Invoke());

        _buyerService.GetByIdAsync(buyer.Id, cancellationToken)
            .Returns(buyer);
        
        // Act
        var buyerResult = await _cachedBuyerService.GetByIdAsync(buyer.Id, cancellationToken);
        
        // Assert
        buyerResult.Should().NotBeNull();
        buyerResult.Should().BeEquivalentTo(buyer);

        await _buyerService.Received().GetByIdAsync(buyer.Id, cancellationToken);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnCachedBuyer_WhenCachedBuyerExists()
    {
        // Arrange
        var buyer = _fixture.Create<Buyer>();
        var cancellationToken = CancellationToken.None;
        
        _cacheService.GetOrCreateAsync(CacheKeys.Buyers.GetByEmail(buyer.Email), Arg.Any<Func<Task<Buyer>>>()!, cancellationToken)!
            .Returns(buyer);
        
        // Act
        var buyerResult = await _cachedBuyerService.GetByEmailAsync(buyer.Email, cancellationToken);
        
        // Assert
        buyerResult.Should().NotBeNull();
        buyerResult.Should().BeEquivalentTo(buyer);

        await _buyerService.DidNotReceive().GetByEmailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnBuyer_AndCreateCache_WhenCachedBuyerDoesNotExist()
    {
        // Arrange
        var buyer = _fixture.Create<Buyer>();
        var cancellationToken = CancellationToken.None;
        
        _cacheService.GetOrCreateAsync(CacheKeys.Buyers.GetByEmail(buyer.Email), Arg.Any<Func<Task<Buyer>>>()!, cancellationToken)!
            .Returns(info => info.Arg<Func<Task<Buyer>>>().Invoke());

        _buyerService.GetByEmailAsync(buyer.Email, cancellationToken)
            .Returns(buyer);

        // Act
        var buyerResult = await _cachedBuyerService.GetByEmailAsync(buyer.Email, cancellationToken);
        
        // Assert
        buyerResult.Should().NotBeNull();
        buyerResult.Should().BeEquivalentTo(buyer);

        await _buyerService.Received().GetByEmailAsync(buyer.Email, cancellationToken);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_AndInvalidateCache_WhenBuyerWasCreated()
    {
        // Arrange
        var buyer = _fixture.Create<Buyer>();
        var cancellationToken = CancellationToken.None;

        _buyerService.CreateAsync(buyer, cancellationToken)
            .Returns(true);

        _publisher.Publish(Arg.Any<CreateBuyerNotification>(), cancellationToken)
            .Returns(ValueTask.CompletedTask);
        
        // Act
        bool isCreated = await _cachedBuyerService.CreateAsync(buyer, cancellationToken);
        
        // Assert
        isCreated.Should().BeTrue();

        await _buyerService.Received().CreateAsync(buyer, cancellationToken);
        await _publisher.Received().Publish(Arg.Any<CreateBuyerNotification>(), cancellationToken);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_AndKeepCache_WhenBuyerWasNotCreated()
    {
        // Arrange
        var buyer = _fixture.Create<Buyer>();
        var cancellationToken = CancellationToken.None;

        _buyerService.CreateAsync(buyer, cancellationToken)
            .Returns(false);
        
        // Act
        bool isCreated = await _cachedBuyerService.CreateAsync(buyer, cancellationToken);
        
        // Assert
        isCreated.Should().BeFalse();

        await _buyerService.Received().CreateAsync(buyer, cancellationToken);
        await _publisher.DidNotReceive().Publish(Arg.Any<INotification>(), cancellationToken);
    }    
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedBuyer_AndInvalidateCache_WhenBuyerWasUpdated()
    {
        // Arrange
        var buyer = _fixture.Create<Buyer>();
        var cancellationToken = CancellationToken.None;

        _buyerService.UpdateAsync(buyer, cancellationToken)
            .Returns(buyer);

        _publisher.Publish(Arg.Any<UpdateBuyerNotification>(), cancellationToken)
            .Returns(ValueTask.CompletedTask);
        
        // Act
        var updatedBuyer = await _cachedBuyerService.UpdateAsync(buyer, cancellationToken);
        
        // Assert
        updatedBuyer.Should().NotBeNull();
        updatedBuyer.Should().BeEquivalentTo(buyer);

        await _buyerService.Received().UpdateAsync(buyer, cancellationToken);
        await _publisher.Received().Publish(Arg.Any<UpdateBuyerNotification>(), cancellationToken);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_AndKeepCache_WhenBuyerWasNotUpdated()
    {
        // Arrange
        var buyer = _fixture.Create<Buyer>();
        var cancellationToken = CancellationToken.None;

        _buyerService.UpdateAsync(buyer, cancellationToken)
            .ReturnsNull();
        
        // Act
        var updatedBuyer = await _cachedBuyerService.UpdateAsync(buyer, cancellationToken);
        
        // Assert
        updatedBuyer.Should().BeNull();

        await _buyerService.Received().UpdateAsync(buyer, cancellationToken);
        await _publisher.DidNotReceive().Publish(Arg.Any<INotification>(), cancellationToken);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_AndInvalidateCache_WhenBuyerWasDeleted()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _buyerService.DeleteByIdAsync(buyerId, cancellationToken)
            .Returns(true);

        _publisher.Publish(Arg.Any<DeleteBuyerNotification>(), cancellationToken)
            .Returns(ValueTask.CompletedTask);
        
        // Act
        bool isDeleted = await _cachedBuyerService.DeleteByIdAsync(buyerId, cancellationToken);
        
        // Assert
        isDeleted.Should().BeTrue();

        await _buyerService.Received().DeleteByIdAsync(buyerId, cancellationToken);
        await _publisher.Received().Publish(Arg.Any<DeleteBuyerNotification>(), cancellationToken);
    }

    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_AndKeepCache_WhenBuyerWasNotDeleted()
    {
        // Arrange
        var buyerId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _buyerService.DeleteByIdAsync(buyerId, cancellationToken)
            .Returns(false);
        
        // Act
        bool isDeleted = await _cachedBuyerService.DeleteByIdAsync(buyerId, cancellationToken);
        
        // Assert
        isDeleted.Should().BeFalse();

        await _buyerService.Received().DeleteByIdAsync(buyerId, cancellationToken);
        await _publisher.DidNotReceive().Publish(Arg.Any<INotification>(), cancellationToken);
    }

}