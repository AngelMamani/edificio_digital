using edificio_digital.Models.Auth;
using edificio_digital.Models.Common;
using edificio_digital.Models.Domain.Auth;
using Microsoft.Extensions.Options;

namespace edificio_digital.Service.Auth;

public class AuthService(
    IAuthRepository authRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService,
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<JwtSettings> jwtSettings) : IAuthService
{
    private readonly JwtSettings _jwt = jwtSettings.Value;

    public async Task<AuthLoginResultDto> LoginAsync(LoginRequestDto request)
    {
        var normalizedEmail = request.Email.Trim();
        var user = await authRepository.GetByEmailAsync(normalizedEmail);

        if (user is null || !passwordHasher.Verify(request.Contrasena, user.Contrasena))
        {
            return new AuthLoginResultDto
            {
                Response = new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "Usuario o contraseña incorrectos."
                }
            };
        }

        var rolPrimario = ResolverRolPrimario(user.Roles);
        if (rolPrimario is null)
        {
            return new AuthLoginResultDto
            {
                Response = new LoginResponseDto
                {
                    IsSuccess = false,
                    Message = "El usuario no tiene un rol vigente asignado."
                }
            };
        }

        var access = jwtTokenService.CreateAccessToken(user);
        var refreshValue = jwtTokenService.CreateRefreshTokenValue();
        var refreshHash = jwtTokenService.HashRefreshToken(refreshValue);
        var refreshExpires = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays);

        await refreshTokenRepository.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = refreshHash,
            ExpiresAt = refreshExpires,
            CreatedAt = DateTime.UtcNow
        });

        return new AuthLoginResultDto
        {
            Response = new LoginResponseDto
            {
                IsSuccess = true,
                Message = $"Bienvenido {user.NombreCompleto}.",
                UserId = user.Id,
                NombreCompleto = user.NombreCompleto,
                Email = user.Email,
                Rol = rolPrimario,
                Roles = user.Roles,
                AccessToken = access.Token,
                AccessTokenExpiresAt = access.ExpiresAt
            },
            RefreshToken = refreshValue,
            RefreshTokenExpiresAt = refreshExpires
        };
    }

    public async Task<RefreshTokenResultDto> RefreshAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return new RefreshTokenResultDto { IsSuccess = false, Message = "Refresh token requerido." };

        var hash = jwtTokenService.HashRefreshToken(refreshToken);
        var stored = await refreshTokenRepository.FindByHashAsync(hash);

        if (stored is null || !stored.IsActive)
            return new RefreshTokenResultDto { IsSuccess = false, Message = "Refresh token inválido o expirado." };

        var user = await authRepository.GetByIdAsync(stored.UserId);
        if (user is null)
            return new RefreshTokenResultDto { IsSuccess = false, Message = "Usuario no encontrado." };

        var newRefreshValue = jwtTokenService.CreateRefreshTokenValue();
        var newRefreshHash = jwtTokenService.HashRefreshToken(newRefreshValue);
        var newRefreshExpires = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays);
        var newRefreshId = Guid.NewGuid();

        stored.RevokedAt = DateTime.UtcNow;
        stored.ReplacedByTokenId = newRefreshId;
        await refreshTokenRepository.UpdateAsync(stored);

        await refreshTokenRepository.AddAsync(new RefreshToken
        {
            Id = newRefreshId,
            UserId = user.Id,
            TokenHash = newRefreshHash,
            ExpiresAt = newRefreshExpires,
            CreatedAt = DateTime.UtcNow
        });

        var access = jwtTokenService.CreateAccessToken(user);

        return new RefreshTokenResultDto
        {
            IsSuccess = true,
            AccessToken = access.Token,
            AccessTokenExpiresAt = access.ExpiresAt,
            NewRefreshToken = newRefreshValue,
            NewRefreshTokenExpiresAt = newRefreshExpires
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken)) return;

        var hash = jwtTokenService.HashRefreshToken(refreshToken);
        var stored = await refreshTokenRepository.FindByHashAsync(hash);
        if (stored is not null && stored.RevokedAt is null)
        {
            stored.RevokedAt = DateTime.UtcNow;
            await refreshTokenRepository.UpdateAsync(stored);
        }
    }

    private static string? ResolverRolPrimario(IEnumerable<string> roles)
    {
        var normalizados = roles.Select(r => r.ToLowerInvariant()).ToHashSet();
        if (normalizados.Contains(AppConstants.Roles.Admin)) return AppConstants.Roles.Admin;
        if (normalizados.Contains(AppConstants.Roles.Solicitante)) return AppConstants.Roles.Solicitante;
        return null;
    }
}
