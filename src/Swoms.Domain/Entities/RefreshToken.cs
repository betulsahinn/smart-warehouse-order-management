using Swoms.Domain.Common;

namespace Swoms.Domain.Entities;

public sealed class RefreshToken : BaseEntity
{
    private RefreshToken()
    {
    }

    public RefreshToken(string token, DateTime expiresAtUtc, string createdByIp)
    {
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
        CreatedByIp = createdByIp;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public string Token { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime ExpiresAtUtc { get; private set; }

    public DateTime? RevokedAtUtc { get; private set; }

    public string CreatedByIp { get; private set; } = string.Empty;

    public string? RevokedByIp { get; private set; }

    public string? ReplacedByToken { get; private set; }

    public Guid ApplicationUserId { get; private set; }

    public ApplicationUser? ApplicationUser { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;

    public bool IsActive => RevokedAtUtc is null && !IsExpired;

    public void Revoke(string revokedByIp, string? replacedByToken = null)
    {
        RevokedAtUtc = DateTime.UtcNow;
        RevokedByIp = revokedByIp;
        ReplacedByToken = replacedByToken;
    }
}
