namespace Swoms.Application.Features.Products;

public sealed record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    decimal UnitPrice,
    int ReorderLevel,
    bool IsActive);

public sealed record CreateProductRequest(
    string Sku,
    string Name,
    string? Description,
    decimal UnitPrice,
    int ReorderLevel);

public sealed record UpdateProductRequest(
    string Name,
    string? Description,
    decimal UnitPrice,
    int ReorderLevel);

public sealed record AdjustStockRequest(Guid WarehouseId, int QuantityDelta, string Reason);

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default);

    Task<ProductDto> GetProductAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ProductDto> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default);

    Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);

    Task AdjustStockAsync(Guid productId, AdjustStockRequest request, CancellationToken cancellationToken = default);
}
