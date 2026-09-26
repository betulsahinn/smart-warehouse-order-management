using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swoms.Application.Common.Interfaces;
using Swoms.Application.Features.Auth;
using Swoms.Domain.Common;
using Swoms.Domain.Entities;

namespace Swoms.Infrastructure.Security;

public sealed class AuthService : IAuthService
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        IRepository<ApplicationUser> users,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IOptions<JwtOptions> jwtOptions)
    {
        _users = users;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _jwtOptions = jwtOptions.Value;
        _passwordHasher = new PasswordHasher<ApplicationUser>();
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, string ipAddress, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existingUser = await _users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
        if (existingUser is not null)
        {
            throw new DomainException("A user with this email already exists.");
        }

        var user = new ApplicationUser(email, request.FullName, string.Empty);
        var passwordHash = _passwordHasher.HashPassword(user, request.Password);
        user = new ApplicationUser(email, request.FullName, passwordHash);

        _users.Add(user);
        var refreshToken = CreateRefreshToken(ipAddress);
        user.AddRefreshToken(refreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return BuildAuthResponse(user, refreshToken);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, string ipAddress, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _users.FirstOrDefaultAsync(
            candidate => candidate.Email == email && candidate.IsActive,
            cancellationToken,
            nameof(ApplicationUser.RefreshTokens))
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (passwordResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var refreshToken = CreateRefreshToken(ipAddress);
        user.AddRefreshToken(refreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return BuildAuthResponse(user, refreshToken);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress, CancellationToken cancellationToken = default)
    {
        var user = await _users.FirstOrDefaultAsync(
            candidate => candidate.RefreshTokens.Any(token => token.Token == request.RefreshToken),
            cancellationToken,
            nameof(ApplicationUser.RefreshTokens))
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        var existingToken = user.RefreshTokens.Single(token => token.Token == request.RefreshToken);
        if (!existingToken.IsActive)
        {
            throw new UnauthorizedAccessException("Refresh token is no longer active.");
        }

        var newRefreshToken = CreateRefreshToken(ipAddress);
        existingToken.Revoke(ipAddress, newRefreshToken.Token);
        user.AddRefreshToken(newRefreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return BuildAuthResponse(user, newRefreshToken);
    }

    private AuthResponse BuildAuthResponse(ApplicationUser user, RefreshToken refreshToken)
    {
        var expiresAt = _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);
        var accessToken = CreateAccessToken(user, expiresAt);

        return new AuthResponse(
            user.Id,
            user.Email,
            user.FullName,
            accessToken,
            expiresAt,
            refreshToken.Token,
            refreshToken.ExpiresAtUtc);
    }

    private string CreateAccessToken(ApplicationUser user, DateTime expiresAtUtc)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken CreateRefreshToken(string ipAddress)
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return new RefreshToken(
            Convert.ToBase64String(bytes),
            _dateTimeProvider.UtcNow.AddDays(_jwtOptions.RefreshTokenDays),
            ipAddress);
    }
}
