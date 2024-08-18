using AutoFixture;
using AutoFixture.Xunit2;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Addresses;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Application.Validators.Orders;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Validators.Orders;

public sealed class CreateOrderRequestValidatorTests
{
    private readonly CreateOrderRequestValidator _createOrderRequestValidator;
    private readonly Fixture _fixture;

    public CreateOrderRequestValidatorTests()
    {
        _createOrderRequestValidator = new CreateOrderRequestValidator();
        _fixture = new Fixture();
    }
    
    
    [Theory]
    [InlineData(null)]
    public async Task InvalidAddress_ShouldThrowValidationError(CreateAddressRequest addressRequest)
    {
        // Arrange
        var request = _fixture.Build<CreateOrderRequest>()
            .With(createOrderRequest => createOrderRequest.Address, addressRequest)
            .Create();

        // Act
        var result = await _createOrderRequestValidator.TestValidateAsync(request);

        // Assert   
        result.ShouldHaveValidationErrorFor(createOrderRequest => createOrderRequest.Address);
    }
    
    
    [Theory, AutoData]
    public async Task ValidProperties_ShouldNotThrowAnyValidationError(CreateAddressRequest addressRequest)
    {
        // Arrange
        var request = _fixture.Build<CreateOrderRequest>()
            .With(createOrderRequest => createOrderRequest.Address, addressRequest)
            .With(createOrderRequest => createOrderRequest.OrderItems, new[]
            {
                new OrderItem
                {
                    Title = "Title",
                    Description = "Description",
                    UnitPrice = 1.4f,
                    Quantity = 1,
                    Discount = 0.3f
                }
            })
            .Create();

        // Act
        var result = await _createOrderRequestValidator.TestValidateAsync(request);

        // Assert   
        result.ShouldNotHaveAnyValidationErrors();
    }
}