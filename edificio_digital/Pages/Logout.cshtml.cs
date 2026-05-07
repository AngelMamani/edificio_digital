using edificio_digital.Models.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace edificio_digital.Pages;

[Authorize]
public class LogoutModel : PageModel
{
    public IActionResult OnGet() => RedirectToPage(AppConstants.Pages.Home);

    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync(AppConstants.Auth.CookieScheme);
        return RedirectToPage(AppConstants.Pages.Home);
    }
}
