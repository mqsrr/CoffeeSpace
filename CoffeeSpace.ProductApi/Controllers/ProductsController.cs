using Asp.Versioning;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Helpers;
using CoffeeSpace.ProductApi.Application.Mapping;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.ProductApi.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1.0)]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet(ApiEndpoints.Products.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllProductsAsync(cancellationToken);
        var responses = products.Select(product => product.ToResponse()); 
        
        return Ok(responses);
    }
    
    [HttpGet(ApiEndpoints.Products.Get)]
    public async Task<IActionResult> GetById([FromRoute] GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.Id.ToString(), cancellationToken);
        return product is not null
            ? Ok(product.ToResponse())
            : NotFound();
    }
    
    [HttpPost(ApiEndpoints.Products.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = request.ToProduct();
        bool created = await _productRepository.CreateProductAsync(product, cancellationToken);

        return created
            ? CreatedAtAction(nameof(GetById), new {id = product.Id}, product.ToResponse())
            : BadRequest();
    }
    
    [HttpPut(ApiEndpoints.Products.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var updatedProduct = await _productRepository.UpdateProductAsync(request.ToProduct(id), cancellationToken);

        return updatedProduct is not null
            ? Ok(updatedProduct.ToResponse())
            : NotFound();
    }
    
    [HttpDelete(ApiEndpoints.Products.Delete)]
    public async Task<IActionResult> Delete([FromRoute] DeleteProductByIdRequest request, CancellationToken cancellationToken)
    {
        bool deleted = await _productRepository.DeleteProductByIdAsync(request.Id.ToString(), cancellationToken);
        return deleted
            ? NoContent()
            : NotFound();
    }
}