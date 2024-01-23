using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.PaymentService.Application.Models;
using CoffeeSpace.PaymentService.Application.Repositories;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PayPalCheckoutSdk.Orders;
using Xunit;

namespace CoffeeSpace.PaymentService.Tests.Repositories;

public sealed class PaymentRepositoryTests
{
    private readonly IPaymentDbContext _dbContext;
    private readonly DbSet<PaypalOrderInformation> _paypalOrderInformationsDbSet;
    private readonly DbSet<Order> _paypalOrdersDbSet;

    private readonly Fixture _fixture;
    private readonly IEnumerable<PaypalOrderInformation> _paypalOrderInformations;
    private readonly IEnumerable<Order> _paypalOrders;

    private readonly PaymentRepository _paymentRepository;

    public PaymentRepositoryTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());

        _paypalOrderInformationsDbSet = _fixture.CreateMany<PaypalOrderInformation>().AsQueryable().BuildMockDbSet();
        _paypalOrdersDbSet = _fixture.CreateMany<Order>().AsQueryable().BuildMockDbSet();

        _dbContext = _fixture.Create<IPaymentDbContext>();
        _dbContext.PaypalOrders.Returns(_paypalOrderInformationsDbSet);
        _dbContext.Orders.Returns(_paypalOrdersDbSet);

        _paypalOrderInformations = _paypalOrderInformationsDbSet.AsEnumerable();
        _paypalOrders = _paypalOrdersDbSet.AsEnumerable();

        _paymentRepository = new PaymentRepository(_dbContext);
    }

    [Fact]
    public async Task GetPaypalOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var expectedOrder = _paypalOrders.First();

        _paypalOrdersDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .Returns(expectedOrder);

        // Act
        var result = await _paymentRepository.GetPaypalOrderByIdAsync(expectedOrder.Id, CancellationToken.None);

        // Assert   
        result.Should().BeEquivalentTo(expectedOrder);
        await _paypalOrdersDbSet.Received().FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetPaypalOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var expectedOrder = _fixture.Create<Order>();
        
        _paypalOrdersDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .ReturnsNull();
        
        // Act
        var result = await _paymentRepository.GetPaypalOrderByIdAsync(expectedOrder.Id, CancellationToken.None);

        // Assert   
        result.Should().BeNull();
        await _paypalOrdersDbSet.Received().FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetByApplicationOrderIdAsync_ShouldReturnOrderInformation_WhenOrderExists()
    {
        // Arrange
        var expectedOrderInformation = _paypalOrderInformations.First();

        _paypalOrderInformationsDbSet.FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>())
            .Returns(expectedOrderInformation);

        
        // Act
        var result = await _paymentRepository.GetByApplicationOrderIdAsync(expectedOrderInformation.Id, CancellationToken.None);

        // Assert   
        result.Should().BeEquivalentTo(expectedOrderInformation);
        await _paypalOrderInformationsDbSet.Received().FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetByApplicationOrderIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var expectedOrderInformation = _fixture.Create<PaypalOrderInformation>();

        // Act
        var result = await _paymentRepository.GetByApplicationOrderIdAsync(expectedOrderInformation.Id, CancellationToken.None);

        // Assert   
        result.Should().BeNull();
        await _paypalOrderInformationsDbSet.Received().FindAsync(Arg.Any<object[]>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CreatePaymentAsync_ShouldReturnTrue_WhenPaymentWasCreated()
    {
        // Arrange
        var createdOrderInformation = _fixture.Create<PaypalOrderInformation>();

        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(1);

        // Act
        bool result = await _paymentRepository.CreatePaymentAsync(createdOrderInformation, CancellationToken.None);

        // Assert   
        result.Should().BeTrue();
        
        await _paypalOrderInformationsDbSet.Received().AddAsync(createdOrderInformation, Arg.Any<CancellationToken>());
        await _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CreatePaymentAsync_ShouldReturnFalse_WhenPaymentWasCreated()
    {
        // Arrange
        var createdOrderInformation = _fixture.Create<PaypalOrderInformation>();

        _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(0);

        // Act
        bool result = await _paymentRepository.CreatePaymentAsync(createdOrderInformation, CancellationToken.None);

        // Assert   
        result.Should().BeFalse();
        
        await _paypalOrderInformationsDbSet.Received().AddAsync(createdOrderInformation, Arg.Any<CancellationToken>());
        await _dbContext.SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}