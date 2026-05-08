using edificio_digital.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace edificio_digital.Controllers.Solicitante;

[ApiController]
[Route("api/solicitante/reservas")]
[Authorize(Policy = AppConstants.Policies.SolicitanteOnly)]
public class ReservasController : ControllerBase
{
    [HttpGet]
    public IActionResult List() => Ok(Array.Empty<object>());
}
