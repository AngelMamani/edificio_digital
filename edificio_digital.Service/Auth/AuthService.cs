using edificio_digital.Models.Auth;
using edificio_digital.Models.Common;
using edificio_digital.Models.Domain.Auth;

namespace edificio_digital.Service.Auth;

public class AuthService(IAuthRepository authRepository, IPasswordHasher passwordHasher) : IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var normalizedEmail = request.Email.Trim();
        var user = await authRepository.GetByEmailAsync(normalizedEmail);

        if (user is null || !passwordHasher.Verify(request.Contrasena, user.Contrasena))
        {
            return new LoginResponseDto
            {
                IsSuccess = false,
                Message = "Usuario o contraseña incorrectos."
            };
        }

        var rolPrimario = ResolverRolPrimario(user.Roles);

        if (rolPrimario is null)
        {
            return new LoginResponseDto
            {
                IsSuccess = false,
                Message = "El usuario no tiene un rol vigente asignado."
            };
        }

        return new LoginResponseDto
        {
            IsSuccess = true,
            Message = $"Bienvenido {user.NombreCompleto}.",
            UserId = user.Id,
            NombreCompleto = user.NombreCompleto,
            Email = user.Email,
            Rol = rolPrimario,
            Roles = user.Roles,
            RedirectPage = ResolverPaginaDestino(rolPrimario),
            Token = $"token-demo-{Guid.NewGuid():N}"
        };
    }

    private static string? ResolverRolPrimario(IEnumerable<string> roles)
    {
        var normalizados = roles.Select(r => r.ToLowerInvariant()).ToHashSet();
        if (normalizados.Contains(AppConstants.Roles.Admin)) return AppConstants.Roles.Admin;
        if (normalizados.Contains(AppConstants.Roles.Solicitante)) return AppConstants.Roles.Solicitante;
        return null;
    }

    private static string ResolverPaginaDestino(string rol) => rol switch
    {
        AppConstants.Roles.Admin => AppConstants.Pages.AdminHome,
        AppConstants.Roles.Solicitante => AppConstants.Pages.SolicitanteHome,
        _ => AppConstants.Pages.Home
    };
}
