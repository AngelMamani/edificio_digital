# edificio_digital (Web)

**Capa Presentation + Composition Root.** Es el host ASP.NET Core: levanta Kestrel, registra dependencias, configura autenticación con cookies y expone Razor Pages + endpoints API.

## Responsabilidad

- **`Program.cs`** — composición DI, registro del `DbContext`, repositorios, servicios de aplicación, esquema de cookie auth, políticas de autorización, mapeo de Razor Pages y endpoints API, ejecución del seeder al iniciar.
- **`Pages/`** — UI Razor Pages, organizada por audiencia:
  - Páginas públicas en la raíz (`/Index`, `/Privacy`, `/Login`, `/AccessDenied`).
  - `Pages/Admin/` — protegida por la política `AdminOnly`.
  - `Pages/Solicitante/` — protegida por la política `SolicitanteOnly`.
- **`Pages/Shared/_Layout*.cshtml`** — los tres layouts: público, admin, solicitante (más `_LayoutLogin`).
- **`wwwroot/`** — assets estáticos.
- **Endpoints API** (`/api/auth/login`, `/api/auth/logout`) — transporte alternativo para futuros consumidores externos. Reutilizan los mismos servicios que las páginas.

## Reglas

- Esta es **la única capa que toca ASP.NET, autenticación, cookies y HTTP.**
- Las páginas inyectan **interfaces de Application** (`IAuthService`, etc.) — nunca repositorios concretos ni `DbContext` directo, salvo para listados puramente de lectura en páginas administrativas (caso `Admin/Usuarios.cshtml.cs`, donde se inyecta `AppDbContext` para mostrar una tabla; si ese consumo crece, conviene mover a un servicio).
- Toda ruta nueva expuesta debe leer su literal de `AppConstants` para mantenerlo centralizado.

## Estructura actual

```
Program.cs                     DI + cookie auth + políticas + seeder
Pages/
  _ViewStart.cshtml            layout default = _LayoutPublic
  Index.cshtml(.cs)            home pública (redirige a panel si está logueado)
  Login.cshtml(.cs)            firma cookie con claims de rol
  Logout.cshtml(.cs)           SignOut
  AccessDenied.cshtml(.cs)     llegada cuando la política deniega
  Privacy.cshtml(.cs)
  Admin/
    _ViewStart.cshtml          layout = _LayoutAdmin
    Index.cshtml(.cs)          panel admin
    Usuarios.cshtml(.cs)       listado de usuarios y roles
  Solicitante/
    _ViewStart.cshtml          layout = _LayoutSolicitante
    Index.cshtml(.cs)
    NuevaReserva.cshtml(.cs)
  Shared/
    _LayoutPublic.cshtml
    _LayoutAdmin.cshtml
    _LayoutSolicitante.cshtml
    _LayoutLogin.cshtml
    _ValidationScriptsPartial.cshtml
```

## Cómo extender

### Añadir una página dentro de un layout existente

Para una nueva página de admin (`/Admin/Reservas`), crea `Pages/Admin/Reservas.cshtml` y `Reservas.cshtml.cs`. La política `AdminOnly` se aplica automáticamente porque `Program.cs` la asigna a toda la carpeta `/Admin`. El layout también se hereda del `_ViewStart.cshtml` de la carpeta — no hace falta declararlo.

```cshtml
@page
@model edificio_digital.Pages.Admin.ReservasModel
@{ ViewData["Title"] = "Reservas"; }

<h1 class="h3">Reservas</h1>
<!-- contenido -->
```

```csharp
public class ReservasModel(IReservaService reservas) : PageModel
{
    public List<ReservaListadoDto> Items { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Items = await reservas.ListarPendientesAsync();
    }
}
```

### Añadir un endpoint API

Suma el literal en `AppConstants` y mapéalo en `Program.cs`. Reutiliza el mismo servicio que la página.

```csharp
// AppConstants.ApiRoutes.Reservas.Crear ya está definido (ver README de Models)
app.MapPost(AppConstants.ApiRoutes.Reservas.Crear,
    async (CrearReservaRequestDto req, ClaimsPrincipal user, IReservaService svc) =>
    {
        var solicitanteId = Guid.Parse(user.FindFirstValue(AppConstants.Claims.UserId)!);
        var resp = await svc.CrearAsync(solicitanteId, req);
        return resp.IsSuccess ? Results.Ok(resp) : Results.BadRequest(resp);
    })
   .RequireAuthorization(AppConstants.Policies.SolicitanteOnly);
```

Por convención, los endpoints `/api/...` exigen autorización explícita y comparten políticas con las páginas.

### Añadir un layout/área nueva

1. Crea `Pages/<Area>/_ViewStart.cshtml` con el `Layout = AppConstants.Layouts.<NombreNuevo>`.
2. Crea el layout en `Pages/Shared/_Layout<NombreNuevo>.cshtml` (puedes copiar `_LayoutAdmin.cshtml`).
3. Define la política y aplícala en `Program.cs`:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppConstants.Policies.NuevaPolicy,
        p => p.RequireRole(AppConstants.Roles.NuevoRol));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/<Area>", AppConstants.Policies.NuevaPolicy);
});
```

4. Añade la constante de rol en `AppConstants.Roles` y siembra el rol/usuario en `DbSeeder`.

### Añadir una nueva regla de redirección post-login

La regla actual está en dos lugares cooperando:

- `AuthService.ResolverPaginaDestino(rol)` decide la `RedirectPage` por rol.
- `LoginModel.RedirectToHomeDeRol()` la usa cuando un usuario ya autenticado vuelve a `/Login`.

Para un nuevo rol, basta con añadir el `case` en `ResolverPaginaDestino` y la rama equivalente en `LoginModel`. Si la matriz de roles crece, vale la pena promover ese mapeo a un único método `RoleHomeResolver` en Application e inyectarlo donde haga falta.

## Qué evitar

- No referencies `AppDbContext` desde una página por comodidad si la operación es escritura o de lógica. Crea (o amplía) un servicio en `Service` y consúmelo.
- No copies cadenas como `"/api/auth/login"`, `"admin"`, `"_LayoutAdmin"`. Todas existen en `AppConstants`.
- No mezcles autorización por convención (`AuthorizeFolder`) con `[Authorize]` por página dentro del mismo árbol — basta una.
