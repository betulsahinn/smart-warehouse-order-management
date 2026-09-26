using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swoms.Domain.Entities;

namespace Swoms.Infrastructure.Persistence.Configurations;

public sealed class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses");
        builder.HasKey(warehouse => warehouse.Id);
        builder.Property(warehouse => warehouse.Code).HasMaxLength(32).IsRequired();
        builder.Property(warehouse => warehouse.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(warehouse => warehouse.Code).IsUnique();
        builder.OwnsOne(warehouse => warehouse.Address, address =>
        {
            address.Property(value => value.Line1).HasMaxLength(300).IsRequired();
            address.Property(value => value.Line2).HasMaxLength(300);
            address.Property(value => value.City).HasMaxLength(100).IsRequired();
            address.Property(value => value.State).HasMaxLength(100).IsRequired();
            address.Property(value => value.PostalCode).HasMaxLength(32).IsRequired();
            address.Property(value => value.Country).HasMaxLength(100).IsRequired();
        });
        builder.Metadata.FindNavigation(nameof(Warehouse.StockItems))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
