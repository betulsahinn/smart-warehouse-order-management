using Swoms.Domain.Common;
using Swoms.Domain.ValueObjects;

namespace Swoms.Domain.Entities;

public sealed class Warehouse : AuditableEntity
{
    private readonly List<StockItem> _stockItems = [];

    private Warehouse()
    {
    }

    public Warehouse(string code, string name, Address address)
    {
        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        Address = address;
    }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public Address Address { get; private set; } = default!;

    public IReadOnlyCollection<StockItem> StockItems => _stockItems.AsReadOnly();
}
