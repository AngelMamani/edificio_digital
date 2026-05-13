using edificio_digital.Entity.Data;
using edificio_digital.Entity.Model.Usuario;
using edificio_digital.Models.Domain.Auth;
using edificio_digital.Models.Domain.Usuarios;
using edificio_digital.Models.Models.Usuarios;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace edificio_digital.Entity.Auth;

public class Users : IUsuario
{
    private readonly AppDbContext _ctx;
    public Users(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<List<UserDto>> IgetUsers()
    {
        return await _ctx.Usuarios
            .Select(x => new UserDto
            {
                Id = x.Id,
                NombreUsuario = x.NombreUsuario,
                Email = x.Email,
                NombreCompleto = x.NombreCompleto,
                Tipo = x.Tipo,
                Activo = x.Activo,
                DependenciaId = x.DependenciaId
            })
            .ToListAsync();
    }

}
