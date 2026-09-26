using Swoms.Domain.Common;

namespace Swoms.Domain.Entities;

public sealed class Product : AuditableEntity
{
    private Product()
    {
    }

    public Product(string sku, string name, string? description, decimal unitPrice, int reorderLevel)
    {
        Sku = sku.Trim().ToUpperInvariant();
        Name = name.Trim();
        Description = description?.Trim();
        UnitPrice = unitPrice;
        ReorderLevel = reorderLevel;
    }

    public string Sku { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public decimal UnitPrice { get; private set; }

    public int ReorderLevel { get; private set; }

    public bool IsActive { get; private set; } = true;

    public void Update(string name, string? description, decimal unitPrice, int reorderLevel)
    {
        Name = name.Trim();
        Description = description?.Trim();
        UnitPrice = unitPrice;
        ReorderLevel = reorderLevel;
    }
}
