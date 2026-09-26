using Swoms.Domain.Common;

namespace Swoms.Domain.Entities;

public sealed class ApplicationUser : AuditableEntity
{
    private readonly List<RefreshToken> _refreshTokens = [];

    private ApplicationUser()
    {
    }

    public ApplicationUser(string email, string fullName, string passwordHash)
    {
        Email = email.Trim().ToLowerInvariant();
        FullName = fullName.Trim();
        PasswordHash = passwordHash;
    }

    public string Email { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    public void AddRefreshToken(RefreshToken refreshToken) => _refreshTokens.Add(refreshToken);

    public void Deactivate() => IsActive = false;
}
