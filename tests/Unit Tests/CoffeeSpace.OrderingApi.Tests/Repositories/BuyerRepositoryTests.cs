using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Repositories;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Repositories;

public sealed class BuyerRepositoryTests
{
    private readonly IOrderingDbContext _dbContext;
    private readonly DbSet<Buyer> _buyersDbSet;
    private readonly IEnumerable<Buyer> _buyers;
    private readonly Fixture _fixture;

    private readonly BuyerRepository _buyerRepository;

    public BuyerRepositoryTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _buyersDbSet = _fixture.CreateMany<Buyer>().AsQueryable().BuildMockDbSet();
        _buyers = _buyersDbSet.AsEnumerable();

        _dbContext = _fixture.Create<IOrderingDbContext>();
        _dbContext.Buyers.Returns(_buyersDbSet);
        
        _buyerRepository = new BuyerRepository(_dbContext);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBuyer_WhenBuyerExists()
    {
        // Arrange
        var expectedBuyer = _buyers.First();

        // Act
        var result = await _buyerRepository.GetByIdAsync(expectedBuyer.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedBuyer);
        _buyersDbSet.Received();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var buyerId = Guid.Empty;

        // Act
        var result = await _buyerRepository.GetByIdAsync(buyerId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _buyersDbSet.Received();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnBuyer_WhenBuyerExists()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        
        // Act
        var result = await _buyerRepository.GetByEmailAsync(expectedBuyer.Email, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedBuyer);
        _buyersDbSet.Received();
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        string buyerEmail = string.Empty;
        
        // Act
        var result = await _buyerRepository.GetByEmailAsync(buyerEmail, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _buyersDbSet.Received();
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
        
        await _buyersDbSet.Received().AddAsync(buyerToCreate, Arg.Any<CancellationToken>());
        await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
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

        await _buyersDbSet.Received().AddAsync(buyerToCreate, Arg.Any<CancellationToken>());
        await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
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
        
        _buyersDbSet.Received().Update(updatedBuyer);
        await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
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
        
        _buyersDbSet.Received().Update(updatedBuyer);
        await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}