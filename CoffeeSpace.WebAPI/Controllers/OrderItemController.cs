using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.WebAPI.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.WebAPI.Controllers;

[ApiController]
[Route("[controller]/")]
public sealed class OrderItemController : ControllerBase
{
    private readonly IRepository<OrderItem> _orderItemRepo;
    
    public OrderItemController(IRepository<OrderItem> orderItemRepo) 
        => _orderItemRepo = orderItemRepo;

    [HttpGet("get")]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<OrderItem> orderItems = await _orderItemRepo.GetAllAsync();

        return Ok(orderItems);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        OrderItem orderItem = await _orderItemRepo.GetByIdAsync(id.ToString());

        return Ok(orderItem);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] OrderItem orderItem)
    {
        await _orderItemRepo.AddAsync(orderItem);

        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] OrderItem orderItem)
    {
        await _orderItemRepo.UpdateAsync(orderItem);

        return Ok();
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        OrderItem orderItem = await _orderItemRepo.GetByIdAsync(id.ToString());
        
        await _orderItemRepo.DeleteAsync(orderItem);

        return Ok();
    }
}