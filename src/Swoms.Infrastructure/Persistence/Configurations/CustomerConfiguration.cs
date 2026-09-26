using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swoms.Domain.Entities;

namespace Swoms.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(customer => customer.Id);
        builder.Property(customer => customer.Name).HasMaxLength(200).IsRequired();
        builder.Property(customer => customer.Email).HasMaxLength(256).IsRequired();
        builder.OwnsOne(customer => customer.ShippingAddress, address =>
        {
            address.Property(value => value.Line1).HasMaxLength(300).IsRequired();
            address.Property(value => value.Line2).HasMaxLength(300);
            address.Property(value => value.City).HasMaxLength(100).IsRequired();
            address.Property(value => value.State).HasMaxLength(100).IsRequired();
            address.Property(value => value.PostalCode).HasMaxLength(32).IsRequired();
            address.Property(value => value.Country).HasMaxLength(100).IsRequired();
        });
    }
}
