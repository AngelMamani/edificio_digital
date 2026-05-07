using edificio_digital.Entity.Model.Usuario;
using edificio_digital.Models.Common;
using edificio_digital.Models.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace edificio_digital.Entity.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IPasswordHasher hasher, CancellationToken ct = default)
    {
        await db.Database.MigrateAsync(ct);

        var rolAdmin = await EnsureRolAsync(db, AppConstants.Roles.Admin, AppConstants.Roles.Display.Admin, ct);
        var rolSolicitante = await EnsureRolAsync(db, AppConstants.Roles.Solicitante, AppConstants.Roles.Display.Solicitante, ct);

        await EnsureUsuarioConRolAsync(
            db,
            hasher,
            email: "admin@edificiodigital.com",
            nombreUsuario: "admin",
            nombreCompleto: "Administrador del sistema",
            tipo: AppConstants.Roles.Display.Admin,
            contrasenaPlana: "admin",
            rol: rolAdmin,
            ct);
    }

    private static async Task<Rol> EnsureRolAsync(AppDbContext db, string codigo, string nombre, CancellationToken ct)
    {
        var rol = await db.Roles.FirstOrDefaultAsync(r => r.Codigo == codigo, ct);
        if (rol is not null) return rol;

        rol = new Rol { Id = Guid.NewGuid(), Codigo = codigo, Nombre = nombre };
        db.Roles.Add(rol);
        await db.SaveChangesAsync(ct);
        return rol;
    }

    private static async Task EnsureUsuarioConRolAsync(
        AppDbContext db,
        IPasswordHasher hasher,
        string email,
        string nombreUsuario,
        string nombreCompleto,
        string tipo,
        string contrasenaPlana,
        Rol rol,
        CancellationToken ct)
    {
        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Email == email, ct);

        if (usuario is null)
        {
            usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                NombreUsuario = nombreUsuario,
                Email = email,
                NombreCompleto = nombreCompleto,
                Tipo = tipo,
                Activo = true,
                Contrasena = hasher.Hash(contrasenaPlana)
            };
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync(ct);
        }
        else
        {
            var contrasenaCambio = !hasher.Verify(contrasenaPlana, usuario.Contrasena);
            var datosCambio = usuario.Tipo != tipo
                || !usuario.Activo
                || usuario.NombreCompleto != nombreCompleto
                || usuario.NombreUsuario != nombreUsuario;

            if (contrasenaCambio || datosCambio)
            {
                if (contrasenaCambio) usuario.Contrasena = hasher.Hash(contrasenaPlana);
                usuario.Tipo = tipo;
                usuario.Activo = true;
                usuario.NombreCompleto = nombreCompleto;
                usuario.NombreUsuario = nombreUsuario;
                await db.SaveChangesAsync(ct);
            }
        }

        var asignado = await db.UsuariosRoles
            .AnyAsync(ur => ur.UsuarioId == usuario.Id && ur.RolId == rol.Id, ct);

        if (!asignado)
        {
            db.UsuariosRoles.Add(new UsuarioRol
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.Id,
                RolId = rol.Id,
                VigenciaDesde = DateTime.UtcNow
            });
            await db.SaveChangesAsync(ct);
        }
    }
}
