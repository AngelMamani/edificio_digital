using edificio_digital.Entity.Data;
using edificio_digital.Models.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace edificio_digital.Entity.Auth;

public class PostgreSqlAuthRepository(AppDbContext dbContext) : IAuthRepository
{
    public async Task<UserCredential?> GetByEmailAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLower();

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

        if (credential is null) return null;

        credential.Roles = await LoadActiveRolesAsync(credential.Id);
        return credential;
    }

    public async Task<UserCredential?> GetByIdAsync(Guid id)
    {
        var credential = await dbContext.Usuarios
            .AsNoTracking()
            .Where(x => x.Id == id && x.Activo)
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

        if (credential is null) return null;

        credential.Roles = await LoadActiveRolesAsync(credential.Id);
        return credential;
    }

    private async Task<List<string>> LoadActiveRolesAsync(Guid userId)
    {
        var ahora = DateTime.UtcNow;
        return await dbContext.UsuariosRoles
            .AsNoTracking()
            .Where(ur => ur.UsuarioId == userId
                && ur.VigenciaDesde <= ahora
                && (ur.VigenciaHasta == null || ur.VigenciaHasta > ahora))
            .Select(ur => ur.Rol.Codigo)
            .ToListAsync();
    }
}
