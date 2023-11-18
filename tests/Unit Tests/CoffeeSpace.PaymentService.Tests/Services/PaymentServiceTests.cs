using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.PaymentService.Messages.Commands;
using CoffeeSpace.PaymentService.Messages.Queries;
using CoffeeSpace.PaymentService.Models;
using FluentAssertions;
using Mediator;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.PaymentService.Tests.Services;

public sealed class PaymentServiceTests
{
    private readonly ISender _sender;
    private readonly Fixture _fixture;

    private readonly PaymentService.Services.PaymentService _paymentService;
    
    public PaymentServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _sender = _fixture.Create<ISender>();
        _paymentService = new PaymentService.Services.PaymentService(_sender);
    }
    
    [Fact]
    public async Task CreateOrderAsync_ShouldReturnOrder_WhenOrderWasCreated()
    {
        // Arrange
        var applicationOrderToCreate = _fixture.Create<Order>();
        var expectedOrder = _fixture.Create<PayPalCheckoutSdk.Orders.Order>();
        
        _sender.Send(Arg.Any<CreatePaypalOrderCommand>(), Arg.Any<CancellationToken>())
            .Returns(expectedOrder);
        
        // Act
        var result = await _paymentService.CreateOrderAsync(applicationOrderToCreate, CancellationToken.None);
        
        // Assert
        result.Should().BeEquivalentTo(expectedOrder);
        await _sender.Received().Send(Arg.Any<CreatePaypalOrderCommand>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CreateOrderAsync_ShouldReturnNull_WhenOrderWasNotCreated()
    {
        // Arrange
        var applicationOrderToCreate = _fixture.Create<Order>();
        
        _sender.Send(Arg.Any<CreatePaypalOrderCommand>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _paymentService.CreateOrderAsync(applicationOrderToCreate, CancellationToken.None);
        
        // Assert
        result.Should().BeNull();
        await _sender.Received().Send(Arg.Any<CreatePaypalOrderCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CapturePaypalPaymentAsync_ShouldReturnCapturedOrder_WhenOrderWasCaptured()
    {
        // Arrange
        var expectedOrder = _fixture.Create<PayPalCheckoutSdk.Orders.Order>();
        
        _sender.Send(Arg.Any<CapturePaypalOrderCommand>(), Arg.Any<CancellationToken>())
            .Returns(expectedOrder);
        
        // Act
        var result = await _paymentService.CapturePaypalPaymentAsync(expectedOrder.Id, CancellationToken.None);
        
        // Assert
        result.Should().BeEquivalentTo(expectedOrder);
        await _sender.Received().Send(Arg.Any<CapturePaypalOrderCommand>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CapturePaypalPaymentAsync_ShouldReturnNull_WhenOrderWasNotCaptured()
    {
        // Arrange
        string orderId = string.Empty;
        
        _sender.Send(Arg.Any<CapturePaypalOrderCommand>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _paymentService.CapturePaypalPaymentAsync(orderId, CancellationToken.None);
        
        // Assert
        result.Should().BeNull();
        await _sender.Received().Send(Arg.Any<CapturePaypalOrderCommand>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetOrderPaymentByOrderIdAsync_ShouldReturnPaypalOrderInformation_WhenOrderInformationExists()
    {
        // Arrange
        var expectedPaypalOrderInformation = _fixture.Create<PaypalOrderInformation>();
        
        _sender.Send(Arg.Any<GetPaypalOrderInformationByOrderIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(expectedPaypalOrderInformation);
        
        // Act
        var result = await _paymentService.GetOrderPaymentByOrderIdAsync(expectedPaypalOrderInformation.Id, CancellationToken.None);
        
        // Assert
        result.Should().BeEquivalentTo(expectedPaypalOrderInformation);
        await _sender.Received().Send(Arg.Any<GetPaypalOrderInformationByOrderIdQuery>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetOrderPaymentByOrderIdAsync_ShouldReturnNull_WhenOrderInformationDoesNotExists()
    {
        // Arrange
        var expectedPaypalOrderInformation = _fixture.Create<PaypalOrderInformation>();
        
        _sender.Send(Arg.Any<GetPaypalOrderInformationByOrderIdQuery>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _paymentService.GetOrderPaymentByOrderIdAsync(expectedPaypalOrderInformation.Id, CancellationToken.None);
        
        // Assert
        result.Should().BeNull();
        await _sender.Received().Send(Arg.Any<GetPaypalOrderInformationByOrderIdQuery>(), Arg.Any<CancellationToken>());
    }
}