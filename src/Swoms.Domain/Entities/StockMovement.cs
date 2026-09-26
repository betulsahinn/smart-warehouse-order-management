using Swoms.Domain.Common;
using Swoms.Domain.Enums;

namespace Swoms.Domain.Entities;

public sealed class StockMovement : AuditableEntity
{
    private StockMovement()
    {
    }

    public StockMovement(Guid productId, Guid warehouseId, int quantity, StockMovementType type, string reason)
    {
        ProductId = productId;
        WarehouseId = warehouseId;
        Quantity = quantity;
        Type = type;
        Reason = reason.Trim();
    }

    public Guid ProductId { get; private set; }

    public Guid WarehouseId { get; private set; }

    public int Quantity { get; private set; }

    public StockMovementType Type { get; private set; }

    public string Reason { get; private set; } = string.Empty;
}
