using edificio_digital.Entity.Data;
using edificio_digital.Models.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace edificio_digital.Entity.Auth;

public class PostgreSqlAuthRepository(AppDbContext dbContext) : IAuthRepository
{
    public async Task<UserCredential?> GetByEmailAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLower();
        var ahora = DateTime.UtcNow;

        var credential = await dbContext.Usuarios
            .AsNoTracking()
            .Where(x => x.Email.ToLower() == normalizedEmail && x.Activo)
            .Select(x => new UserCredential
            {
                Id = x.Id,
                NombreUsuario = x.NombreUsuario,
                Email = x.Email,
                NombreCompleto = x.NombreCompleto,
                TipoUsuario = x.Tipo,
                Activo = x.Activo,
                Contrasena = x.Contrasena
            })
            .FirstOrDefaultAsync();

        if (credential is null)
        {
            return null;
        }

        credential.Roles = await dbContext.UsuariosRoles
            .AsNoTracking()
            .Where(ur => ur.UsuarioId == credential.Id
                && ur.VigenciaDesde <= ahora
                && (ur.VigenciaHasta == null || ur.VigenciaHasta > ahora))
            .Select(ur => ur.Rol.Codigo)
            .ToListAsync();

        return credential;
    }
}
