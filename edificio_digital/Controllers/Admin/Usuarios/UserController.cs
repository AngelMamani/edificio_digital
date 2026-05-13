using edificio_digital.Models.Common;
using edificio_digital.Models.Domain.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace edificio_digital.Controllers.Admin.Usuarios
{
    [ApiController]
    [Route("api/admin/users")]
    //[Authorize(Policy = AppConstants.Roles.Admin)]
    public class UserController : ControllerBase
    {
        private readonly IUsuario _usuario;
        public UserController(IUsuario usuario)
        {
            _usuario = usuario;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var lista = await _usuario.IgetUsers();

            return Ok(lista);
        }

        //[HttpGet("list2")]
        //public IActionResult List2() => Ok(Array.Empty<object>());

    }

}

