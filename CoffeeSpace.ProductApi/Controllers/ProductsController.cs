using Asp.Versioning;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
using CoffeeSpace.ProductApi.Application.Extensions;
using CoffeeSpace.ProductApi.Application.Helpers;
using CoffeeSpace.ProductApi.Application.Mapping;
using CoffeeSpace.ProductApi.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeSpace.ProductApi.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1.0)]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet(ApiEndpoints.Products.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductsRequest request, CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllProductsAsync(request, cancellationToken);
        var count = await _productService.GetCountAsync(cancellationToken);

        var response = products.Select(x => x.ToResponse()).ToPagedList(request.Page, request.PageSize, count);
        return Ok(response);
    }
    
    [HttpGet(ApiEndpoints.Products.Get)]
    public async Task<IActionResult> GetById([FromRoute] GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductByIdAsync(request.Id, cancellationToken);
        
        return product is not null
            ? Ok(product)
            : NotFound();
    }
    
    [HttpPost(ApiEndpoints.Products.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = request.ToProduct();
        var created = await _productService.CreateProductAsync(product, cancellationToken);

        return created
            ? CreatedAtAction(nameof(GetById), new {id = product.Id}, product.ToResponse())
            : BadRequest();
    }
    
    [HttpPut(ApiEndpoints.Products.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var updatedProduct = await _productService.UpdateProductAsync(request.ToProduct(id), cancellationToken);

        return updatedProduct is not null
            ? Ok(updatedProduct.ToResponse())
            : NotFound();
    }
    
    [HttpDelete(ApiEndpoints.Products.Delete)]
    public async Task<IActionResult> Delete([FromRoute] DeleteProductByIdRequest request, CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteProductByIdAsync(request.Id, cancellationToken);

        return deleted
            ? Ok()
            : NotFound();
    }
}