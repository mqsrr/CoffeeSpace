using Asp.Versioning;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Mapping;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.OrderingApi.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1.0)]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet(ApiEndpoints.Orders.GetAll)]
    public async Task<IActionResult> GetAllOrdersByBuyerId([FromRoute] Guid buyerId, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllByBuyerIdAsync(buyerId.ToString(), cancellationToken);
        var response = orders.Select(x => x.ToResponse());
        
        return Ok(response);
    }
    
    [HttpGet(ApiEndpoints.Orders.Get)]
    public async Task<IActionResult> GetOrderByBuyerId([FromRoute] Guid buyerId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(id.ToString(), buyerId.ToString(), cancellationToken);

        return order is not null
            ? Ok(order.ToResponse())
            : NotFound();
    }

    [HttpPost(ApiEndpoints.Orders.Create)]
    public async Task<IActionResult> CreateOrder([FromRoute] Guid buyerId, [FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = request.ToOrder(buyerId);
        var created = await _orderService.CreateAsync(order, cancellationToken);

        return created
            ? CreatedAtAction(nameof(GetOrderByBuyerId), new {buyerId, order.Id}, order.ToResponse())
            : BadRequest();
    }

    [HttpPut(ApiEndpoints.Orders.Update)]
    public async Task<IActionResult> UpdateOrder([FromRoute] Guid buyerId, [FromRoute] Guid id, [FromBody] UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        var updatedOrder = await _orderService.UpdateAsync(request.ToOrder(id, buyerId), cancellationToken);

        return updatedOrder is not null
            ? Ok(updatedOrder.ToResponse())
            : BadRequest();
    }
    
    [HttpDelete(ApiEndpoints.Orders.Delete)]
    public async Task<IActionResult> DeleteOrder([FromRoute] Guid buyerId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _orderService.DeleteByIdAsync(id.ToString(), buyerId.ToString(), cancellationToken);

        return deleted
            ? Ok()
            : NotFound();
    }
}