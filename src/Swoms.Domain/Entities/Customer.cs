using Swoms.Domain.Common;
using Swoms.Domain.ValueObjects;

namespace Swoms.Domain.Entities;

public sealed class Customer : AuditableEntity
{
    private Customer()
    {
    }

    public Customer(string name, string email, Address shippingAddress)
    {
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        ShippingAddress = shippingAddress;
    }

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public Address ShippingAddress { get; private set; } = default!;
}
