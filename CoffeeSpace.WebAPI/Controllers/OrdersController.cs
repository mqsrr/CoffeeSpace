using CoffeeSpace.Application;
using CoffeeSpace.Application.Models.Orders;
using CoffeeSpace.Application.Repositories.Interfaces;
using CoffeeSpace.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.WebAPI.Controllers;

[ApiController]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepo;
    
    public OrdersController(IOrderRepository orderRepo) =>
        _orderRepo = orderRepo;

    [HttpGet(ApiEndpoints.Orders.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Order> orderItems = await _orderRepo.GetAllAsync();

        return Ok(orderItems);
    }

    [HttpGet(ApiEndpoints.Orders.Get)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        Order? order = await _orderRepo.GetByIdAsync(id.ToString());

        return order is not null
            ? Ok(order)
            : NotFound();
    }

    [HttpPost(ApiEndpoints.Orders.Create)]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        bool isSuccess = await _orderRepo.CreateAsync(order);

        return isSuccess
            ? CreatedAtAction(nameof(GetById),new {id = order.Id} ,order)
            : BadRequest();
    }

    [HttpPut(ApiEndpoints.Orders.Update)]
    public async Task<IActionResult> Update([FromBody] Order order)
    {
        bool isSuccess = await _orderRepo.UpdateAsync(order);

        return isSuccess
            ? Ok()
            : NotFound();
    }

    [HttpDelete(ApiEndpoints.Orders.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        bool isSuccess = await _orderRepo.DeleteByIdAsync(id.ToString());

        return isSuccess
            ? Ok()
            : NotFound();
    }
}