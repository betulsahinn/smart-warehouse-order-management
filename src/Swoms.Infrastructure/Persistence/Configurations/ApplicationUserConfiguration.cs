using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swoms.Domain.Entities;

namespace Swoms.Infrastructure.Persistence.Configurations;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Email).HasMaxLength(256).IsRequired();
        builder.Property(user => user.FullName).HasMaxLength(200).IsRequired();
        builder.Property(user => user.PasswordHash).HasMaxLength(1000).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();
        builder.Metadata.FindNavigation(nameof(ApplicationUser.RefreshTokens))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.HasMany(user => user.RefreshTokens)
            .WithOne(token => token.ApplicationUser)
            .HasForeignKey(token => token.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
