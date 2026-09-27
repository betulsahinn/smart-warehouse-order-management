using Swoms.Domain.Common;

namespace Swoms.Domain.Entities;

public sealed class StockItem : AuditableEntity
{
    private StockItem()
    {
    }

    public StockItem(Guid productId, Guid warehouseId, int quantityOnHand)
    {
        if (quantityOnHand < 0)
        {
            throw new DomainException("Quantity on hand cannot be negative.");
        }

        ProductId = productId;
        WarehouseId = warehouseId;
        QuantityOnHand = quantityOnHand;
    }

    public Guid ProductId { get; private set; }

    public Product? Product { get; private set; }

    public Guid WarehouseId { get; private set; }

    public Warehouse? Warehouse { get; private set; }

    public int QuantityOnHand { get; private set; }

    public int ReservedQuantity { get; private set; }

    public Guid Version { get; private set; } = Guid.NewGuid();

    public int AvailableQuantity => QuantityOnHand - ReservedQuantity;

    public void Adjust(int delta)
    {
        var nextQuantity = QuantityOnHand + delta;
        if (nextQuantity < ReservedQuantity)
        {
            throw new DomainException("Quantity on hand cannot be less than reserved quantity.");
        }

        QuantityOnHand = nextQuantity;
    }

    public void Reserve(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Reservation quantity must be greater than zero.");
        }

        if (AvailableQuantity < quantity)
        {
            throw new DomainException("Insufficient stock is available for reservation.");
        }

        ReservedQuantity += quantity;
    }
}
