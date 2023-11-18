using System.Runtime.InteropServices.ComTypes;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Services;

public sealed class BuyerServiceTests
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IEnumerable<Buyer> _buyers;
    private readonly Fixture _fixture;

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
        await _buyerRepository.Received().GetByEmailAsync(expectedBuyer.Email, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _buyerRepository.GetByEmailAsync(expectedBuyer.Email, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _buyerService.GetByEmailAsync(expectedBuyer.Email, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        await _buyerRepository.Received().GetByEmailAsync(expectedBuyer.Email, Arg.Any<CancellationToken>());
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
        await _buyerRepository.Received().GetByIdAsync(expectedBuyer.Id, Arg.Any<CancellationToken>());
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
        await _buyerRepository.Received().GetByIdAsync(expectedBuyer.Id, Arg.Any<CancellationToken>());
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
        await _buyerRepository.Received().CreateAsync(buyerToCreate, Arg.Any<CancellationToken>());
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
        await _buyerRepository.Received().CreateAsync(buyerToCreate, Arg.Any<CancellationToken>());
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
        await _buyerRepository.Received().UpdateAsync(updatedBuyer, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenBuyerWasNotDeleted()
    {
        // Arrange
        var buyerToDelete = _buyers.First();

        _buyerRepository.GetByIdAsync(buyerToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(buyerToDelete);
        
        _buyerRepository.DeleteByIdAsync(buyerToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(false);
        
        // Act
        bool result = await _buyerService.DeleteByIdAsync(Guid.Parse(buyerToDelete.Id), CancellationToken.None);
 
        // Assert
        result.Should().BeFalse();
        await _buyerRepository.Received().DeleteByIdAsync(buyerToDelete.Id, Arg.Any<CancellationToken>());
    }
}