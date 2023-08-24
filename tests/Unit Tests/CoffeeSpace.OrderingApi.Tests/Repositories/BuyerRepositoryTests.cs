using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Repositories;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using CoffeeSpace.OrderingApi.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Repositories;

public sealed class BuyerRepositoryTests : IClassFixture<OrderingDbContextFixture>
{
    private readonly IOrderingDbContext _dbContext;
    private readonly DbSet<Buyer> _buyersDbSet;
    private readonly IEnumerable<Buyer> _buyers;
    private readonly Fixture _fixture;

    private readonly BuyerRepository _buyerRepository;

    public BuyerRepositoryTests(OrderingDbContextFixture dbContextFixture)
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _buyersDbSet = dbContextFixture.Buyers;
        _buyers = _buyersDbSet.AsEnumerable();

        _dbContext = dbContextFixture.DbContext;
        _buyerRepository = new BuyerRepository(_dbContext);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var expectedBuyer = _buyers.First();

        _buyersDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _buyerRepository.GetByIdAsync(expectedBuyer.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var expectedBuyer = _fixture.Create<Buyer>();
        
        // Act
        var result = await _buyerRepository.GetByEmailAsync(expectedBuyer.Email, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenBuyerWasCreated()
    {
        // Arrange
        var buyerToCreate = _fixture.Create<Buyer>();

        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        
        // Act
        bool result = await _buyerRepository.CreateAsync(buyerToCreate, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenBuyerWasNotCreated()
    {
        // Arrange
        var buyerToCreate = _fixture.Create<Buyer>();

        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(0);
        
        // Act
        bool result = await _buyerRepository.CreateAsync(buyerToCreate, CancellationToken.None);

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
        
        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        
        // Act
        var result = await _buyerRepository.UpdateAsync(updatedBuyer, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(updatedBuyer);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenBuyerWasNotUpdated()
    {
        // Arrange
        var updatedBuyer = _buyers.First();
        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(0);
        
        // Act
        var result = await _buyerRepository.UpdateAsync(updatedBuyer, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}