using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.OrderingApi.Application.Mapping;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Services;

public sealed class OrderServiceTests
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IOrderRepository _orderRepository;
    private readonly IEnumerable<Order> _orders;
    private readonly IHubContext<OrderingHub, IOrderingHub> _hubContext;
    private readonly Fixture _fixture;

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
        _hubContext = _fixture.Create<IHubContext<OrderingHub, IOrderingHub>>();
        
        _orderService = new OrderService(_orderRepository, _publishEndpoint, _hubContext);
    }

    [Fact]
    public async Task GetAllByBuyerIdAsync_ShouldReturnAllOrders()
    {
        // Arrange
        var orders = _orders.Take(2).ToList();
        _orderRepository.GetAllByBuyerIdAsync(orders[0].BuyerId, Arg.Any<CancellationToken>())
            .Returns(orders);
        
        // Act
        var result = await _orderService.GetAllByBuyerIdAsync(Guid.Parse(orders[0].BuyerId), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(orders);
        await _orderRepository.Received().GetAllByBuyerIdAsync(orders[0].BuyerId, Arg.Any<CancellationToken>());
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
        await _orderRepository.Received().GetByIdAsync(expectedOrder.Id, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var order = _fixture.Build<Order>()
            .With(order => order.Id, Guid.NewGuid().ToString())
            .With(order => order.BuyerId, Guid.NewGuid().ToString())
            .Create();
        
        _orderRepository.GetByIdAsync(order.Id, Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _orderService.GetByIdAsync(Guid.Parse(order.Id), Guid.Parse(order.BuyerId), CancellationToken.None);
 
        // Assert
        result.Should().BeNull();
        await _orderRepository.Received().GetByIdAsync(order.Id, Arg.Any<CancellationToken>());
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

        await _orderRepository.Received().CreateAsync(orderToCreate, Arg.Any<CancellationToken>());
        await _hubContext.Clients.Received().Groups(Arg.Any<string[]>()).OrderCreated(orderToCreate.ToResponse());
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

        _hubContext.Clients.DidNotReceive();
        await _orderRepository.Received().CreateAsync(orderToCreate, Arg.Any<CancellationToken>());
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
        await _orderRepository.Received().DeleteByIdAsync(orderToDelete.Id, Arg.Any<CancellationToken>());
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
        await _orderRepository.Received().DeleteByIdAsync(orderToDelete.Id, Arg.Any<CancellationToken>());
    }
}