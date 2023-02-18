using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.WebAPI.Services.Repository;
using CoffeeSpace.WebAPI.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepo;
    
    public OrderController(IOrderRepository orderRepo) =>
        _orderRepo = orderRepo;

    [HttpGet("get")]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Order> orderItems = await _orderRepo.GetAllAsync();

        return Ok(orderItems);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Order orderItem = await _orderRepo.GetByIdAsync(id.ToString());

        return Ok(orderItem);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] Order order)
    {
        await _orderRepo.AddAsync(order);

        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Order order)
    {
        await _orderRepo.UpdateAsync(order);

        return Ok();
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Order order = await _orderRepo.GetByIdAsync(id.ToString()); 
        
        await _orderRepo.DeleteAsync(order);

        return Ok();
    }
}