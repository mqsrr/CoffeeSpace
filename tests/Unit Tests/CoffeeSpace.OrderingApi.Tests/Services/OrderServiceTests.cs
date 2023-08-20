using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Commands.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Mediator.Queries.Orders;
using CoffeeSpace.OrderingApi.Application.Services;
using FluentAssertions;
using Mediator;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Services;

public sealed class OrderServiceTests
{
    private readonly ISender _sender;
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

        _sender = _fixture.Create<ISender>();
        _orderService = new OrderService(_sender);
    }

    [Fact]
    public async Task GetAllByBuyerIdAsync_ShouldReturnAllOrders()
    {
        // Arrange
        var expectedOrder = _orders.First();
        _sender.Send(Arg.Any<GetAllOrdersByBuyerIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(new []{expectedOrder});

        // Act
        var result = await _orderService.GetAllByBuyerIdAsync(Guid.Parse(expectedOrder.BuyerId), CancellationToken.None);

        // Assert
        result.Should().ContainEquivalentOf(expectedOrder);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var expectedOrder = _orders.First();
        _sender.Send(Arg.Any<GetOrderByIdQuery>(), Arg.Any<CancellationToken>())
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
        _sender.Send(Arg.Any<GetOrderByIdQuery>(), Arg.Any<CancellationToken>())
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
        _sender.Send(Arg.Any<CreateOrderCommand>(), Arg.Any<CancellationToken>())
            .Returns(true);

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
        _sender.Send(Arg.Any<CreateOrderCommand>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _orderService.CreateAsync(orderToCreate, CancellationToken.None);
 
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

        _sender.Send(Arg.Any<UpdateOrderCommand>(), Arg.Any<CancellationToken>())
            .Returns(updatedOrder);

        // Act
        var result = await _orderService.UpdateAsync(updatedOrder, CancellationToken.None);
 
        // Assert
        result.Should().BeEquivalentTo(updatedOrder);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenOrderWasNotUpdated()
    {
        // Arrange
        var updatedOrder = _orders.First();
        _sender.Send(Arg.Any<UpdateOrderCommand>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _orderService.UpdateAsync(updatedOrder, CancellationToken.None);
 
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenOrderWasDeleted()
    {
        // Arrange
        var orderToDelete = _orders.First();
        _sender.Send(Arg.Any<DeleteOrderByIdCommand>(), Arg.Any<CancellationToken>())
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
        _sender.Send(Arg.Any<DeleteOrderByIdCommand>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _orderService.DeleteByIdAsync(Guid.Parse(orderToDelete.Id), Guid.Parse(orderToDelete.BuyerId), CancellationToken.None);
 
        // Assert
        result.Should().BeFalse();
    }
}