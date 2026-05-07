using edificio_digital.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace edificio_digital.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole(AppConstants.Roles.Admin))
                return RedirectToPage(AppConstants.Pages.AdminHome);
            if (User.IsInRole(AppConstants.Roles.Solicitante))
                return RedirectToPage(AppConstants.Pages.SolicitanteHome);
        }
        return Page();
    }
}
