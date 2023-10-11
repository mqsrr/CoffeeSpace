using AutoFixture;
using AutoFixture.AutoNSubstitute;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Services;
using FluentAssertions;
using Mediator;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Services;

public sealed class BuyerServiceTests
{
    private readonly ISender _sender;
    private readonly Fixture _fixture;
    private readonly IEnumerable<Buyer> _buyers;
    
    private readonly BuyerService _buyerService;
    
    public BuyerServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
        _buyers = _fixture.Build<Buyer>()
            .With(buyer => buyer.Id, Guid.NewGuid().ToString())
            .CreateMany();

        _sender = _fixture.Create<ISender>();
        _buyerService = new BuyerService();
    }
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnBuyer_WhenBuyerExists()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _sender.Send(Arg.Any<GetBuyerByEmailQuery>(), Arg.Any<CancellationToken>())
            .Returns(expectedBuyer);

        // Act
        var result = await _buyerService.GetByEmailAsync(expectedBuyer.Email, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedBuyer);
    }
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _sender.Send(Arg.Any<GetBuyerByEmailQuery>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _buyerService.GetByEmailAsync(expectedBuyer.Email, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBuyer_WhenBuyerExists()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _sender.Send(Arg.Any<GetBuyerByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(expectedBuyer);

        // Act
        var result = await _buyerService.GetByIdAsync(Guid.Parse(expectedBuyer.Id), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedBuyer);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var expectedBuyer = _buyers.First();
        _sender.Send(Arg.Any<GetBuyerByIdQuery>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _buyerService.GetByIdAsync(Guid.Parse(expectedBuyer.Id), CancellationToken.None);
 
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenBuyerWasCreated()
    {
        // Arrange
        var buyerToCreate = _fixture.Create<Buyer>();
        _sender.Send(Arg.Any<CreateBuyerCommand>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        bool result = await _buyerService.CreateAsync(buyerToCreate, CancellationToken.None);
 
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenBuyerWasNotCreated()
    {
        // Arrange
        var buyerToCreate = _fixture.Create<Buyer>();
        _sender.Send(Arg.Any<CreateBuyerCommand>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _buyerService.CreateAsync(buyerToCreate, CancellationToken.None);
 
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

        _sender.Send(Arg.Any<UpdateBuyerCommand>(), Arg.Any<CancellationToken>())
            .Returns(updatedBuyer);

        // Act
        var result = await _buyerService.UpdateAsync(updatedBuyer, CancellationToken.None);
 
        // Assert
        result.Should().BeEquivalentTo(updatedBuyer);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenBuyerWasNotUpdated()
    {
        // Arrange
        var updatedBuyer = _buyers.First();
        _sender.Send(Arg.Any<UpdateBuyerCommand>(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _buyerService.UpdateAsync(updatedBuyer, CancellationToken.None);
 
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenBuyerWasDeleted()
    {
        // Arrange
        var buyerToDelete = _buyers.First();
        _sender.Send(Arg.Any<DeleteBuyerByIdCommand>(), Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        bool result = await _buyerService.DeleteByIdAsync(Guid.Parse(buyerToDelete.Id), CancellationToken.None);
 
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenBuyerWasNotDeleted()
    {
        // Arrange
        var buyerToDelete = _buyers.First();
        _sender.Send(Arg.Any<DeleteBuyerByIdCommand>(), Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        bool result = await _buyerService.DeleteByIdAsync(Guid.Parse(buyerToDelete.Id), CancellationToken.None);
 
        // Assert
        result.Should().BeFalse();
    }
}