using AutoFixture;
using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using CoffeeSpace.PaymentService.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.PaymentService.Tests.Repositories;

public sealed class PaymentHistoryRepositoryTests
{
    private readonly IPaymentDbContext _dbContext;
    private readonly DbSet<PaymentHistory> _paymentHistoriesDbSet;
    private readonly Fixture _fixture;
    private readonly IEnumerable<PaymentHistory> _paymentHistories;

    private readonly PaymentHistoryRepository _paymentHistoryRepository;
    
    public PaymentHistoryRepositoryTests()
    {
        _fixture = new Fixture();
        _paymentHistories = _fixture.CreateMany<PaymentHistory>();
        
        _dbContext = Substitute.For<IPaymentDbContext>();
        _paymentHistoriesDbSet = _paymentHistories.AsQueryable().BuildMockDbSet();
        _dbContext.PaymentHistories.Returns(_paymentHistoriesDbSet);

        _paymentHistoryRepository = new PaymentHistoryRepository(_dbContext);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPaymentHistories()
    {
        // Arrange
        
        // Act
        var result = await _paymentHistoryRepository.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(_paymentHistories);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnPaymentHistory_WhenPaymentHistoryExists()
    {
        // Arrange
        var expectedPaymentHistory = _paymentHistories.First();
        _paymentHistoriesDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .Returns(expectedPaymentHistory);
        
        // Act
        var result = await _paymentHistoryRepository.GetByIdAsync(expectedPaymentHistory.PaymentId, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedPaymentHistory);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPaymentHistoryDoesNotExist()
    {
        // Arrange
        var expectedPaymentHistory = _paymentHistories.First();
        _paymentHistoriesDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _paymentHistoryRepository.GetByIdAsync(expectedPaymentHistory.PaymentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenPaymentHistoryWasCreated()
    {
        // Arrange
        var paymentHistoryToCreate = _fixture.Create<PaymentHistory>();
        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        
        // Act
        bool result = await _paymentHistoryRepository.CreateAsync(paymentHistoryToCreate, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenPaymentHistoryWasNotCreated()
    {
        // Arrange
        var paymentHistoryToCreate = _fixture.Create<PaymentHistory>();
        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(0);
        
        // Act
        bool result = await _paymentHistoryRepository.CreateAsync(paymentHistoryToCreate, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}