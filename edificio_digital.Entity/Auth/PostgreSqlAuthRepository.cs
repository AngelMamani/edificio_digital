using edificio_digital.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace edificio_digital.Entity.Auth;

public class PostgreSqlAuthRepository(AppDbContext dbContext) : IAuthRepository
{
    public async Task<UserCredentialEntity?> GetByEmailAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLower();

        return await dbContext.Usuarios
            .AsNoTracking()
            .Where(x => x.Email.ToLower() == normalizedEmail && x.Activo)
            .Select(x => new UserCredentialEntity
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
    }
}

