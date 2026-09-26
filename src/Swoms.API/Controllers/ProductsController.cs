using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swoms.Application.Features.Products;

namespace Swoms.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts(CancellationToken cancellationToken)
    {
        return Ok(await _productService.GetProductsAsync(cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _productService.GetProductAsync(id, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.CreateProductAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _productService.UpdateProductAsync(id, request, cancellationToken));
    }

    [HttpPost("{id:guid}/stock-adjustments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AdjustStock(Guid id, AdjustStockRequest request, CancellationToken cancellationToken)
    {
        await _productService.AdjustStockAsync(id, request, cancellationToken);
        return NoContent();
    }
}
