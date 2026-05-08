using edificio_digital.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace edificio_digital.Controllers.Admin;

[ApiController]
[Route("api/admin/usuarios")]
[Authorize(Policy = AppConstants.Policies.AdminOnly)]
public class UsuariosController : ControllerBase
{
    [HttpGet]
    public IActionResult List() => Ok(Array.Empty<object>());
}
