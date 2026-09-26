using Microsoft.AspNetCore.Mvc;
using Swoms.Application.Features.Auth;

namespace Swoms.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.RegisterAsync(request, GetIpAddress(), cancellationToken);
        return CreatedAtAction(nameof(Register), new { id = response.UserId }, response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _authService.LoginAsync(request, GetIpAddress(), cancellationToken));
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _authService.RefreshTokenAsync(request, GetIpAddress(), cancellationToken));
    }

    private string GetIpAddress()
    {
        return Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor)
            ? forwardedFor.ToString()
            : HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
