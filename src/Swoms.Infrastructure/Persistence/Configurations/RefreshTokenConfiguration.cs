using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swoms.Domain.Entities;

namespace Swoms.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(token => token.Id);
        builder.Property(token => token.Token).HasMaxLength(512).IsRequired();
        builder.Property(token => token.CreatedByIp).HasMaxLength(64).IsRequired();
        builder.Property(token => token.RevokedByIp).HasMaxLength(64);
        builder.Property(token => token.ReplacedByToken).HasMaxLength(512);
        builder.HasIndex(token => token.Token).IsUnique();
    }
}
