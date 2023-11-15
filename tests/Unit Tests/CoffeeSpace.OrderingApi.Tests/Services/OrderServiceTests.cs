using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services;
using FluentAssertions;
using MassTransit;
using Mediator;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Services;

public sealed class OrderServiceTests
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IOrderRepository _orderRepository;
    private readonly Fixture _fixture;
    private readonly IEnumerable<Order> _orders;
    
    private readonly OrderService _orderService;
    
    public OrderServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        _orders = _fixture.Build<Order>()
            .With(order => order.Id, Guid.NewGuid().ToString())
            .With(order => order.BuyerId, Guid.NewGuid().ToString())
            .CreateMany();

        _publishEndpoint = _fixture.Create<IPublishEndpoint>();
        _orderRepository = _fixture.Create<IOrderRepository>();
        
        _orderService = new OrderService(_orderRepository, _publishEndpoint);
    }

    [Fact]
    public async Task GetAllByBuyerIdAsync_ShouldReturnAllOrders()
    {
        // Arrange
        var orders = _orders.Take(2).ToList();
        _orderRepository.GetAllByBuyerIdAsync(orders[0].BuyerId, CancellationToken.None)
            .Returns(orders);
        
        // Act
        var result = await _orderService.GetAllByBuyerIdAsync(Guid.Parse(orders[0].BuyerId), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(orders);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var expectedOrder = _orders.First();
        _orderRepository.GetByIdAsync(expectedOrder.Id, Arg.Any<CancellationToken>())
            .Returns(expectedOrder);

        // Act
        var result = await _orderService.GetByIdAsync(Guid.Parse(expectedOrder.Id), Guid.Parse(expectedOrder.BuyerId), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedOrder);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var expectedOrder = _orders.First();
        _orderRepository.GetByIdAsync(expectedOrder.Id, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _orderService.GetByIdAsync(Guid.Parse(expectedOrder.Id), Guid.Parse(expectedOrder.BuyerId), CancellationToken.None);
 
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenOrderWasCreated()
    {
        // Arrange
        var orderToCreate = _fixture.Create<Order>();
        
        _orderRepository.CreateAsync(orderToCreate, Arg.Any<CancellationToken>())
            .Returns(true);

        _publishEndpoint.Publish<SubmitOrder>(Arg.Any<object>())
            .Returns(Task.CompletedTask);

        // Act
        bool result = await _orderService.CreateAsync(orderToCreate, CancellationToken.None);
 
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenOrderWasNotCreated()
    {
        // Arrange
        var orderToCreate = _fixture.Create<Order>();
        _orderRepository.CreateAsync(orderToCreate, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _orderService.CreateAsync(orderToCreate, CancellationToken.None);
 
        // Assert
        result.Should().BeFalse();
    }
    
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenOrderWasDeleted()
    {
        // Arrange
        var orderToDelete = _orders.First();
        _orderRepository.DeleteByIdAsync(orderToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        bool result = await _orderService.DeleteByIdAsync(Guid.Parse(orderToDelete.Id), Guid.Parse(orderToDelete.BuyerId), CancellationToken.None);
 
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenOrderWasNotDeleted()
    {
        // Arrange
        var orderToDelete = _orders.First();
        _orderRepository.DeleteByIdAsync(orderToDelete.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _orderService.DeleteByIdAsync(Guid.Parse(orderToDelete.Id), Guid.Parse(orderToDelete.BuyerId), CancellationToken.None);
 
        // Assert
        result.Should().BeFalse();
    }
}