using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Services;

public sealed class BuyerServiceTests
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IBuyerRepository _buyerRepository;
    private readonly Fixture _fixture;
    private readonly IEnumerable<Buyer> _buyers;
    
    private readonly BuyerService _buyerService;
    
    public BuyerServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        _buyers = _fixture.Build<Buyer>()
            .With(buyer => buyer.Id, Guid.NewGuid().ToString())
            .CreateMany();

        _sendEndpointProvider = _fixture.Create<ISendEndpointProvider>();
        _buyerRepository = _fixture.Create<IBuyerRepository>();

        _buyerService = new BuyerService(_buyerRepository, _sendEndpointProvider);
    }
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnBuyer_WhenBuyerExists()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _buyerRepository.GetByEmailAsync(expectedBuyer.Email, Arg.Any<CancellationToken>())
            .Returns(expectedBuyer);
        
        // Act
        var result = await _buyerService.GetByEmailAsync(expectedBuyer.Email, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedBuyer);
    }
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _buyerRepository.GetByEmailAsync(expectedBuyer.Email, CancellationToken.None)
            .ReturnsNull();

        // Act
        var result = await _buyerService.GetByEmailAsync(expectedBuyer.Email, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBuyer_WhenBuyerExists()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _buyerRepository.GetByIdAsync(expectedBuyer.Id, CancellationToken.None)
            .Returns(expectedBuyer);

        // Act
        var result = await _buyerService.GetByIdAsync(Guid.Parse(expectedBuyer.Id), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedBuyer);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _buyerRepository.GetByIdAsync(expectedBuyer.Id, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _buyerService.GetByIdAsync(Guid.Parse(expectedBuyer.Id), CancellationToken.None);
 
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenBuyerWasCreated()
    {
        // Arrange
        var buyerToCreate = _fixture.Create<Buyer>();
        _buyerRepository.CreateAsync(buyerToCreate, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        bool result = await _buyerService.CreateAsync(buyerToCreate, CancellationToken.None);
 
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenBuyerWasNotCreated()
    {
        // Arrange
        var buyerToCreate = _fixture.Create<Buyer>();
        _buyerRepository.CreateAsync(buyerToCreate, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _buyerService.CreateAsync(buyerToCreate, CancellationToken.None);
 
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedBuyer_WhenBuyerWasUpdated()
    {
        // Arrange
        var buyerToUpdate = _buyers.First();
        var updatedBuyer = _fixture.Build<Buyer>()
            .With(buyer => buyer.Id, buyerToUpdate.Id)
            .Create();

        _buyerRepository.UpdateAsync(updatedBuyer, CancellationToken.None)
            .Returns(updatedBuyer);
        
        // Act
        var result = await _buyerService.UpdateAsync(updatedBuyer, CancellationToken.None);
 
        // Assert
        result.Should().BeEquivalentTo(updatedBuyer);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenBuyerWasNotUpdated()
    {
        // Arrange
        var updatedBuyer = _buyers.First();
        _buyerRepository.UpdateAsync(updatedBuyer, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _buyerService.UpdateAsync(updatedBuyer, CancellationToken.None);
 
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenBuyerWasDeleted()
    {
        // Arrange
        var buyerToDelete = _buyers.First();
        
        _buyerRepository.DeleteByIdAsync(buyerToDelete.Id, CancellationToken.None)
            .Returns(true);

        
        _sendEndpointProvider.Send(Arg.Any<object>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        

        // Act
        bool result = await _buyerService.DeleteByIdAsync(Guid.Parse(buyerToDelete.Id), CancellationToken.None);
 
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenBuyerWasNotDeleted()
    {
        // Arrange
        var buyerToDelete = _buyers.First();
        _buyerRepository.DeleteByIdAsync(buyerToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(false);
        // Act
        bool result = await _buyerService.DeleteByIdAsync(Guid.Parse(buyerToDelete.Id), CancellationToken.None);
 
        // Assert
        result.Should().BeFalse();
    }
}