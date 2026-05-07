using System.Security.Claims;
using edificio_digital.Entity.Auth;
using edificio_digital.Entity.Data;
using edificio_digital.Models.Auth;
using edificio_digital.Models.Common;
using edificio_digital.Models.Domain.Auth;
using edificio_digital.Service.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));

builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IAuthRepository, PostgreSqlAuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services
    .AddAuthentication(AppConstants.Auth.CookieScheme)
    .AddCookie(AppConstants.Auth.CookieScheme, options =>
    {
        options.Cookie.Name = AppConstants.Auth.CookieName;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = AppConstants.Pages.Login;
        options.AccessDeniedPath = AppConstants.Pages.AccessDenied;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppConstants.Policies.AdminOnly, p => p.RequireRole(AppConstants.Roles.Admin));
    options.AddPolicy(AppConstants.Policies.SolicitanteOnly, p => p.RequireRole(AppConstants.Roles.Solicitante));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", AppConstants.Policies.AdminOnly);
    options.Conventions.AuthorizeFolder("/Solicitante", AppConstants.Policies.SolicitanteOnly);
    options.Conventions.AllowAnonymousToPage(AppConstants.Pages.Login);
    options.Conventions.AllowAnonymousToPage(AppConstants.Pages.AccessDenied);
    options.Conventions.AllowAnonymousToPage(AppConstants.Pages.Home);
    options.Conventions.AllowAnonymousToPage("/Privacy");
    options.Conventions.AllowAnonymousToPage("/Error");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost(AppConstants.ApiRoutes.Auth.Login, async (LoginRequestDto request, IAuthService authService) =>
{
    var response = await authService.LoginAsync(request);
    return response.IsSuccess ? Results.Ok(response) : Results.Unauthorized();
});

app.MapPost(AppConstants.ApiRoutes.Auth.Logout, async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(AppConstants.Auth.CookieScheme);
    return Results.Ok();
}).RequireAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    await DbSeeder.SeedAsync(db, hasher);
}

app.Run();
