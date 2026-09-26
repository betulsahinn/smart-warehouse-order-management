namespace Swoms.Application.Features.Warehouses;

public sealed record WarehouseDto(
    Guid Id,
    string Code,
    string Name,
    string Line1,
    string City,
    string State,
    string PostalCode,
    string Country);

public sealed record CreateWarehouseRequest(
    string Code,
    string Name,
    string Line1,
    string? Line2,
    string City,
    string State,
    string PostalCode,
    string Country);

public interface IWarehouseService
{
    Task<IReadOnlyList<WarehouseDto>> GetWarehousesAsync(CancellationToken cancellationToken = default);

    Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request, CancellationToken cancellationToken = default);
}
