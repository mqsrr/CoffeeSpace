using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Repositories;

public sealed class OrderRepositoryTests
{
    private readonly IOrderingDbContext _dbContext;
    private readonly DbSet<Order> _ordersDbSet;

    private readonly Fixture _fixture;
    private readonly IEnumerable<Order> _orders;

    private readonly OrderRepository _orderRepository;

    public OrderRepositoryTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _ordersDbSet = _fixture.CreateMany<Order>().AsQueryable().BuildMockDbSet();
        var buyersDbSet = _fixture.CreateMany<Buyer>().AsQueryable().BuildMockDbSet();
        
        _orders = _ordersDbSet.AsEnumerable();
        _dbContext = _fixture.Create<IOrderingDbContext>();
        
        _dbContext.Orders.Returns(_ordersDbSet);
        _dbContext.Buyers.Returns(buyersDbSet);
        
        _orderRepository = new OrderRepository(_dbContext);
    }

    [Fact]
    public async Task GetAllByBuyerIdAsync_ShouldReturnAllOrders()
    {
        // Arrange
        var expectedOrder = _orders.First();
        var buyerId = expectedOrder.BuyerId;

        // Act
        var result = await _orderRepository.GetAllByBuyerIdAsync(buyerId, CancellationToken.None);

        // Assert
        result.Should().ContainEquivalentOf(expectedOrder);
        _orders.Received();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var expectedOrder = _orders.First();
        
        // Act
        var result = await _orderRepository.GetByIdAsync(expectedOrder.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedOrder);
        _ordersDbSet.Received();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderId = Guid.Empty;
        
        // Act
        var result = await _orderRepository.GetByIdAsync(orderId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _ordersDbSet.Received();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenOrderWasCreated()
    {
        // Arrange
        var orderToCreate = _fixture.Create<Order>();

        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        
        // Act
        bool result = await _orderRepository.CreateAsync(orderToCreate, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        await _ordersDbSet.Received().AddAsync(orderToCreate, Arg.Any<CancellationToken>());
        await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenOrderWasNotCreated()
    {
        // Arrange
        var orderToCreate = _fixture.Create<Order>();
        
        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(0);
        
        // Act
        bool result = await _orderRepository.CreateAsync(orderToCreate, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        
        await _ordersDbSet.Received().AddAsync(orderToCreate, Arg.Any<CancellationToken>());
        await _dbContext.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}