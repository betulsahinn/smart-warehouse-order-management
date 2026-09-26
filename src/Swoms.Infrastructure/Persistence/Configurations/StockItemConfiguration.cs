using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swoms.Domain.Entities;

namespace Swoms.Infrastructure.Persistence.Configurations;

public sealed class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.ToTable("StockItems");
        builder.HasKey(stock => stock.Id);
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
