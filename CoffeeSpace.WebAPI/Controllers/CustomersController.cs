using AutoMapper;
using CoffeeSpace.Application;
using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Repositories.Interfaces;
using CoffeeSpace.Contracts.Requests.Customer;
using CoffeeSpace.Contracts.Responses.Customers;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.WebAPI.Controllers;

[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly IMapper _mapper;
    public CustomersController(IRepository<Customer> customerRepo, IMapper mapper)
    {
        _customerRepo = customerRepo;
        _mapper = mapper;
    }

    [HttpGet(ApiEndpoints.Customers.Get)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        Customer? customer = await _customerRepo.GetByIdAsync(id.ToString());

        if (customer is null)
            return NotFound();

        CustomerResponse response = _mapper.Map<CustomerResponse>(customer);
        
        return Ok(response);
    }
    
    [HttpGet(ApiEndpoints.Customers.GetOrders)]
    public async Task<IActionResult> GetCustomerOrders([FromRoute] Guid id)
    {
        Customer? customer = await _customerRepo.GetByIdAsync(id.ToString());

        if (customer is null)
            return NotFound();

        CustomerResponse response = _mapper.Map<CustomerResponse>(customer);

        return Ok(response);
    }

    [HttpPost(ApiEndpoints.Customers.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        Customer customer = _mapper.Map<Customer>(request);
        
        bool isSuccess = await _customerRepo.CreateAsync(customer);

        if (!isSuccess)
            return BadRequest();

        CustomerResponse response = _mapper.Map<CustomerResponse>(customer);

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, response);
    }

    [HttpPut(ApiEndpoints.Customers.Update)]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerRequest request)
    {
        Customer customer = _mapper.Map<Customer>(request);
        
        bool isSuccess = await _customerRepo.UpdateAsync(customer);

        if (!isSuccess)
            return BadRequest();

        CustomerResponse response = _mapper.Map<CustomerResponse>(customer);

        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Customers.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        bool isSuccess = await _customerRepo.DeleteByIdAsync(id.ToString());

        if (!isSuccess)
            return BadRequest();

        return isSuccess
            ? Ok()
            : NotFound();
    }
}