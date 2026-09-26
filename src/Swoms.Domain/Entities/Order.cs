using Swoms.Domain.Common;
using Swoms.Domain.Enums;

namespace Swoms.Domain.Entities;

public sealed class Order : AuditableEntity
{
    private readonly List<OrderItem> _items = [];

    private Order()
    {
    }

    public Order(Guid customerId, Guid warehouseId)
    {
        CustomerId = customerId;
        WarehouseId = warehouseId;
        OrderNumber = $"SO-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
    }

    public string OrderNumber { get; private set; } = string.Empty;

    public Guid CustomerId { get; private set; }

    public Customer? Customer { get; private set; }

    public Guid WarehouseId { get; private set; }

    public Warehouse? Warehouse { get; private set; }

    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public decimal TotalAmount => _items.Sum(item => item.LineTotal);

    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Order item quantity must be greater than zero.");
        }

        _items.Add(new OrderItem(productId, productName, quantity, unitPrice));
    }

    public void Confirm()
    {
        if (!_items.Any())
        {
            throw new DomainException("An order must have at least one item before it can be confirmed.");
        }

        Status = OrderStatus.Confirmed;
    }
}
