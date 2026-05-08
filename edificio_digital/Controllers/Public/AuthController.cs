using System.Security.Claims;
using edificio_digital.Models.Auth;
using edificio_digital.Models.Common;
using edificio_digital.Service.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace edificio_digital.Controllers.Public;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, IOptions<JwtSettings> jwtOptions) : ControllerBase
{
    private readonly JwtSettings _jwt = jwtOptions.Value;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await authService.LoginAsync(request);
        if (!result.Response.IsSuccess)
            return Unauthorized(result.Response);

        SetRefreshCookie(result.RefreshToken!, result.RefreshTokenExpiresAt!.Value);
        return Ok(result.Response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies[_jwt.RefreshCookieName];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "Refresh token ausente." });

        var result = await authService.RefreshAsync(refreshToken);
        if (!result.IsSuccess)
        {
            DeleteRefreshCookie();
            return Unauthorized(new { message = result.Message });
        }

        SetRefreshCookie(result.NewRefreshToken!, result.NewRefreshTokenExpiresAt!.Value);
        return Ok(new
        {
            accessToken = result.AccessToken,
            accessTokenExpiresAt = result.AccessTokenExpiresAt
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies[_jwt.RefreshCookieName];
        if (!string.IsNullOrEmpty(refreshToken))
            await authService.LogoutAsync(refreshToken);

        DeleteRefreshCookie();
        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        Guid? userId = Guid.TryParse(User.FindFirst(AppConstants.Claims.UserId)?.Value, out var uid) ? uid : null;
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

        return Ok(new
        {
            UserId = userId,
            Email = User.FindFirst(AppConstants.Claims.Email)?.Value,
            NombreCompleto = User.FindFirst(AppConstants.Claims.NombreCompleto)?.Value,
            Roles = roles
        });
    }

    private void SetRefreshCookie(string value, DateTime expires) =>
        Response.Cookies.Append(_jwt.RefreshCookieName, value, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Strict,
            Path = _jwt.RefreshCookiePath,
            Expires = expires
        });

    private void DeleteRefreshCookie() =>
        Response.Cookies.Delete(_jwt.RefreshCookieName, new CookieOptions
        {
            Path = _jwt.RefreshCookiePath,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Strict
        });
}
