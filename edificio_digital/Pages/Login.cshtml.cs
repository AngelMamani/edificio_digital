using System.Security.Claims;
using edificio_digital.Models.Auth;
using edificio_digital.Models.Common;
using edificio_digital.Service.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace edificio_digital.Pages;

[AllowAnonymous]
public class LoginModel(IAuthService authService) : PageModel
{
    [BindProperty]
    public LoginRequestDto LoginInput { get; set; } = new();

    public string Message { get; private set; } = string.Empty;
    public bool IsSuccess { get; private set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToHomeDeRol();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Message = "Completa los campos requeridos.";
            IsSuccess = false;
            return Page();
        }

        var result = await authService.LoginAsync(LoginInput);
        if (!result.IsSuccess)
        {
            Message = result.Message;
            IsSuccess = false;
            return Page();
        }

        var claims = new List<Claim>
        {
            new(AppConstants.Claims.UserId, result.UserId?.ToString() ?? string.Empty),
            new(AppConstants.Claims.Email, result.Email ?? string.Empty),
            new(AppConstants.Claims.NombreCompleto, result.NombreCompleto ?? string.Empty),
            new(ClaimTypes.NameIdentifier, result.UserId?.ToString() ?? string.Empty),
            new(ClaimTypes.Name, result.NombreCompleto ?? result.Email ?? string.Empty)
        };
        claims.AddRange(result.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var identity = new ClaimsIdentity(claims, AppConstants.Auth.CookieScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            AppConstants.Auth.CookieScheme,
            principal,
            new AuthenticationProperties { IsPersistent = false });

        return RedirectToPage(result.RedirectPage ?? AppConstants.Pages.Home);
    }

    private IActionResult RedirectToHomeDeRol()
    {
        if (User.IsInRole(AppConstants.Roles.Admin))
            return RedirectToPage(AppConstants.Pages.AdminHome);
        if (User.IsInRole(AppConstants.Roles.Solicitante))
            return RedirectToPage(AppConstants.Pages.SolicitanteHome);
        return RedirectToPage(AppConstants.Pages.Home);
    }
}
