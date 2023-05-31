using Asp.Versioning;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;
using CoffeeSpace.OrderingApi.Application.Helpers;
using CoffeeSpace.OrderingApi.Application.Mapping;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CoffeeSpace.OrderingApi.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1.0)]
public sealed class BuyersController : ControllerBase
{
    private readonly IBuyerService _buyerService;

    public BuyersController(IBuyerService buyerService)
    {
        _buyerService = buyerService;
    }

    [HttpGet(ApiEndpoints.Buyer.Get)]
    public async Task<IActionResult> GetBuyerById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var buyer = await _buyerService.GetByIdAsync(id.ToString(), cancellationToken);

        return buyer is not null
            ? Ok(buyer.ToResponse())
            : NotFound();
    }
    
    [HttpGet(ApiEndpoints.Buyer.GetWithEmail)]
    public async Task<IActionResult> GetBuyerByEmail([FromRoute] string email, CancellationToken cancellationToken)
    {
        var buyer = await _buyerService.GetByEmailAsync(email, cancellationToken);

        return buyer is not null
            ? Ok(buyer.ToResponse())
            : NotFound();
    }

    [HttpPost(ApiEndpoints.Buyer.Create)]
    public async Task<IActionResult> CreateBuyer([FromBody] CreateBuyerRequest request, CancellationToken cancellationToken)
    {
        var buyer = request.ToBuyer();
        var created = await _buyerService.CreateAsync(buyer, cancellationToken);
 
        return created
            ? CreatedAtAction(nameof(GetBuyerById), new { id = buyer.Id }, buyer.ToResponse())
            : BadRequest();
    }

    [HttpPut(ApiEndpoints.Buyer.Update)]
    public async Task<IActionResult> UpdateBuyer([FromRoute] Guid id, [FromBody] UpdateBuyerRequest request,CancellationToken cancellationToken)
    {
        var updatedBuyer = await _buyerService.UpdateAsync(request.ToBuyer(id), cancellationToken);
        
        return updatedBuyer is not null
            ? Ok(updatedBuyer.ToResponse())
            : NotFound();
    }

    [HttpDelete(ApiEndpoints.Buyer.Delete)]
    public async Task<IActionResult> DeleteBuyerById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _buyerService.DeleteByIdAsync(id.ToString(), cancellationToken);

        return deleted
            ? Ok()
            : NotFound();
    }
}