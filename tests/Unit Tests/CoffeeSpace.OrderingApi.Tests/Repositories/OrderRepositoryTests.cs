using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using CoffeeSpace.OrderingApi.Tests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Repositories;

public sealed class OrderRepositoryTests : IClassFixture<OrderingDbContextFixture>
{
    private readonly IOrderingDbContext _dbContext;
    private readonly DbSet<Order> _ordersDbSet;
    private readonly DbSet<Buyer> _buyersDbSet;
    
    private readonly Fixture _fixture;
    private readonly IEnumerable<Order> _orders;

    private readonly OrderRepository _orderRepository;

    public OrderRepositoryTests(OrderingDbContextFixture dbContextFixture)
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _ordersDbSet = dbContextFixture.Orders;
        _orders = _ordersDbSet.AsEnumerable();
        _buyersDbSet = dbContextFixture.Buyers;

        _dbContext = dbContextFixture.DbContext;
        _orderRepository = new OrderRepository(_dbContext);
    }

    [Fact]
    public async Task GetAllByBuyerIdAsync_ShouldReturnAllOrders()
    {
        // Arrange
        var expectedOrder = _orders.First();
        string buyerId = expectedOrder.BuyerId;

        // Act
        var result = await _orderRepository.GetAllByBuyerIdAsync(buyerId, CancellationToken.None);

        // Assert
        result.Should().ContainEquivalentOf(expectedOrder);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var expectedOrder = _orders.First();

        _ordersDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _orderRepository.GetByIdAsync(expectedOrder.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenOrderWasNotCreated()
    {
        // Arrange
        var orderToCreate = _fixture.Create<Order>();
        
        _buyersDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        bool result = await _orderRepository.CreateAsync(orderToCreate, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedOrder_WhenOrderWasUpdated()
    {
        // Arrange
        var orderToUpdate = _orders.First();
        var updatedOrder = _fixture.Build<Order>()
            .With(order => order.Id, orderToUpdate.Id)
            .Create();
        
        _ordersDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .Returns(orderToUpdate);

        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        
        // Act
        var result = await _orderRepository.UpdateAsync(updatedOrder, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(updatedOrder);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenOrderWasNotUpdated()
    {
        // Arrange
        var updatedOrder = _orders.First();
        
        _ordersDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _orderRepository.UpdateAsync(updatedOrder, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}