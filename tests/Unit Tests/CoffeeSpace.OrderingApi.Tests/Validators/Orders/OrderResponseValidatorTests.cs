using AutoFixture;
using AutoFixture.Xunit2;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Addressses;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using CoffeeSpace.OrderingApi.Application.Validators.Orders;
using FluentValidation.TestHelper;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Validators.Orders;

public sealed class OrderResponseValidatorTests
{
    private readonly OrderResponseValidator _orderResponseValidator;
    private readonly Fixture _fixture;

    public OrderResponseValidatorTests()
    {
        _orderResponseValidator = new OrderResponseValidator();
        _fixture = new Fixture();
    }
    
    [Theory]
    [InlineData(null)]
    public async Task InvalidAddress_ShouldThrowValidationError(AddressResponse addressResponse)
    {
        // Arrange
        var response = _fixture.Build<OrderResponse>()
            .With(orderResponse => orderResponse.Address, addressResponse)
            .Create();

        // Act
        var result = await _orderResponseValidator.TestValidateAsync(response);

        // Assert   
        result.ShouldHaveValidationErrorFor(createOrderRequest => createOrderRequest.Address);
    }
}