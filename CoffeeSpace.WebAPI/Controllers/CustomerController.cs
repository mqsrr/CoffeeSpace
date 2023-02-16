using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.WebAPI.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IRepository<Customer> _customerRepo;

    public CustomerController(IRepository<Customer> customerRepo) 
        => _customerRepo = customerRepo;
    
    [HttpGet("get")]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Customer> customers = await _customerRepo.GetAllAsync();

        return Ok(customers);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Customer customer = await _customerRepo.GetByIdAsync(id.ToString());

        return Ok(customer);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] Customer customer)
    {
        await _customerRepo.AddAsync(customer);

        return Ok();
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Customer customer)
    {
        await _customerRepo.UpdateAsync(customer);

        return Ok();
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Customer customer = await _customerRepo.GetByIdAsync(id.ToString()); 
        
        await _customerRepo.DeleteAsync(customer);

        return Ok();
    }
}