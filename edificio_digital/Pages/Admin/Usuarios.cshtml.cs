using edificio_digital.Entity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace edificio_digital.Pages.Admin;

public class UsuariosModel(AppDbContext db) : PageModel
{
    public record UsuarioRow(string NombreCompleto, string Email, bool Activo, List<string> Roles);

    public List<UsuarioRow> Usuarios { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Usuarios = await db.Usuarios
            .AsNoTracking()
            .Select(u => new UsuarioRow(
                u.NombreCompleto,
                u.Email,
                u.Activo,
                u.UsuarioRoles.Select(ur => ur.Rol.Codigo).ToList()))
            .ToListAsync();
    }
}
