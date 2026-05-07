using edificio_digital.Models.Auth;
using edificio_digital.Service.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace edificio_digital.Pages;

public class LoginModel(IAuthService authService) : PageModel
{
    [BindProperty]
    public LoginRequestDto LoginInput { get; set; } = new();

    public LoginResponseDto? LoginResult { get; private set; }

    public string Message { get; private set; } = string.Empty;

    public bool IsSuccess { get; private set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Message = "Completa los campos requeridos.";
            IsSuccess = false;
            return Page();
        }

        LoginResult = await authService.LoginAsync(LoginInput);
        Message = LoginResult.Message;
        IsSuccess = LoginResult.IsSuccess;

        return Page();
    }
}
