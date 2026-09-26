using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swoms.Application.Features.Warehouses;

namespace Swoms.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<WarehouseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<WarehouseDto>>> GetWarehouses(CancellationToken cancellationToken)
    {
        return Ok(await _warehouseService.GetWarehousesAsync(cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<WarehouseDto>> CreateWarehouse(CreateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var warehouse = await _warehouseService.CreateWarehouseAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetWarehouses), new { id = warehouse.Id }, warehouse);
    }
}
