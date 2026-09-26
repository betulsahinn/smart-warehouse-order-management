using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swoms.Domain.Entities;

namespace Swoms.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(order => order.Id);
        builder.Property(order => order.OrderNumber).HasMaxLength(64).IsRequired();
        builder.HasIndex(order => order.OrderNumber).IsUnique();
        builder.Ignore(order => order.TotalAmount);
        builder.Metadata.FindNavigation(nameof(Order.Items))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.HasMany(order => order.Items)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
