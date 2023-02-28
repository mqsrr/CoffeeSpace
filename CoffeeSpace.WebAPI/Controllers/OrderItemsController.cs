using AutoMapper;
using CoffeeSpace.Application;
using CoffeeSpace.Application.Models.Orders;
using CoffeeSpace.Application.Repositories.Interfaces;
using CoffeeSpace.Contracts.Requests.OrderItem;
using CoffeeSpace.Contracts.Responses.OrderItems;
using CoffeeSpace.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.WebAPI.Controllers;

[ApiController]
public sealed class OrderItemsController : ControllerBase
{
    private readonly IRepository<OrderItem> _orderItemRepo;
    private readonly IMapper _mapper;
    
    public OrderItemsController(IRepository<OrderItem> orderItemRepo, IMapper mapper)
    {
        _orderItemRepo = orderItemRepo;
        _mapper = mapper;
    }

    [HttpGet(ApiEndpoints.OrderItems.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<OrderItem> orderItems = await _orderItemRepo.GetAllAsync();

        OrderItemsResponse response = new OrderItemsResponse(
            OrderItemResponses: orderItems.Select(x => _mapper.Map<OrderItemResponse>(x)).ToList());
        
        return Ok(orderItems);
    }

    [HttpGet(ApiEndpoints.OrderItems.Get)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        OrderItem? orderItem = await _orderItemRepo.GetByIdAsync(id.ToString());

        if (orderItem is null) 
            NotFound();
        
        OrderItemResponse response = _mapper.Map<OrderItemResponse>(orderItem);

        return Ok(response);    
    }

    [HttpPost(ApiEndpoints.OrderItems.Create)]
    public async Task<IActionResult> Create([FromBody] CreateOrderItemRequest request)
    {
        OrderItem orderItem = _mapper.Map<OrderItem>(request);
        
        bool isSuccess = await _orderItemRepo.CreateAsync(orderItem);

        if (!isSuccess)
            return BadRequest();

        OrderItemResponse response = _mapper.Map<OrderItemResponse>(orderItem);
        
        return CreatedAtAction(nameof(GetById),new {id = response.Id} ,response);
    }

    [HttpPut(ApiEndpoints.OrderItems.Update)]
    public async Task<IActionResult> Update([FromBody] UpdateOrderItemRequest request)
    {
        OrderItem orderItem = _mapper.Map<OrderItem>(request);
        
        bool isSuccess = await _orderItemRepo.UpdateAsync(orderItem);

        if (!isSuccess)
            return NotFound();

        OrderItemResponse response = _mapper.Map<OrderItemResponse>(orderItem);

        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.OrderItems.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        bool isSuccess = await _orderItemRepo.DeleteByIdAsync(id.ToString());

        return isSuccess
            ? Ok()
            : NotFound();
    }
}