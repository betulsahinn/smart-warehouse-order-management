using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swoms.Domain.Entities;

namespace Swoms.Infrastructure.Persistence.Configurations;

public sealed class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.ToTable("StockItems", table =>
            table.HasCheckConstraint(
                "CK_StockItems_ValidQuantities",
                "\"QuantityOnHand\" >= 0 AND \"ReservedQuantity\" >= 0 AND \"ReservedQuantity\" <= \"QuantityOnHand\""));
        builder.HasKey(stock => stock.Id);
        builder.Property(stock => stock.Version).IsConcurrencyToken();
        builder.HasIndex(stock => new { stock.ProductId, stock.WarehouseId }).IsUnique();
        builder.HasOne(stock => stock.Product)
            .WithMany()
            .HasForeignKey(stock => stock.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(stock => stock.Warehouse)
            .WithMany(warehouse => warehouse.StockItems)
            .HasForeignKey(stock => stock.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
