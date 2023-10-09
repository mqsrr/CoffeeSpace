using Asp.Versioning;
using CoffeeSpace.ProductApi.Application.Contracts.Requests;
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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllProductsAsync(cancellationToken);
        return Ok(products);
    }
        
    [HttpGet(ApiEndpoints.Products.GetPaged)]
    public async Task<IActionResult> GetPaged([FromQuery]GetPagedProductsRequest request, CancellationToken cancellationToken)
    {
        var pagedProducts = await _productService.GetAllProductsAsync(request.Page, request.PageSize, cancellationToken);
        return Ok(pagedProducts);
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
        bool created = await _productService.CreateProductAsync(product, cancellationToken);

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
        bool deleted = await _productService.DeleteProductByIdAsync(request.Id, cancellationToken);
        return deleted
            ? NoContent()
            : NotFound();
    }
}