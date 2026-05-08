# edificio_digital (Host)

**Capa Presentation server-side + Composition Root.** Es el host ASP.NET Core: levanta Kestrel, registra dependencias, configura autenticación con cookies, expone los **controladores HTTP (API)** y monta el **shell Blazor Web App** que sirve la WASM al navegador.

> La UI ya **no** vive aquí. Todas las páginas se centralizaron en `edificio_digital.Client` (Blazor WebAssembly). Este proyecto solo es API + shell.

## Responsabilidad

- **`Program.cs`** — composición DI, registro del `DbContext`, repositorios, servicios de aplicación, **autenticación JWT (`AddJwtBearer`)**, políticas de autorización, **middleware de security headers + CSP**, mapeo de controladores y Razor Components, ejecución del seeder al iniciar.
- **`Controllers/`** — endpoints HTTP organizados por rol consumidor:
  - `Controllers/Public/` — endpoints anónimos o de uso transversal (`AuthController`).
  - `Controllers/Admin/` — protegidos con `[Authorize(Policy = AppConstants.Policies.AdminOnly)]`.
  - `Controllers/Solicitante/` — protegidos con `[Authorize(Policy = AppConstants.Policies.SolicitanteOnly)]`.
- **`Components/`** — shell Blazor Web App:
  - `App.razor` — documento HTML root con `<base>`, fonts Geist, Bootstrap CDN, `<HeadOutlet />` y `<Routes @rendermode="InteractiveWebAssembly" />`.
  - `Routes.razor` — `<Router>` con `AuthorizeRouteView` que delega rutas y assemblies adicionales al ensamblado de `.Client`.
  - `_Imports.razor` — usings server-side incluyendo `@using static Microsoft.AspNetCore.Components.Web.RenderMode`.
- **`wwwroot/css/design-tokens.css`** — variables CSS y utilidades `ed-*` que sobreescriben Bootstrap (paleta lila, tipografía Geist, sombras, glass nav, status pills). Documentado en `.claude/skills/edificio-digital-design/SKILL.md`.

## Reglas

- Esta es **la única capa que toca ASP.NET, autenticación, cookies y HTTP.**
- Los controladores inyectan **interfaces de Application** (`IAuthService`, etc.) — nunca repositorios concretos ni `DbContext` directo. La interfaz declara el caso de uso.
- Toda ruta nueva se declara en `AppConstants.ApiRoutes` (cuando proceda) y se referencia desde el atributo `[Route]` del controlador o desde el cliente.
- Los controladores son **delgados**: validación de modelo (vía `[ApiController]`), llamada al servicio, mapeo de respuesta a `IActionResult`. Cero lógica de negocio.
- Las redirecciones de cookie en respuestas a `/api/*` se interceptan para devolver `401`/`403` en vez de HTML — el cliente WASM lo necesita para reaccionar correctamente.

## Estructura actual

```
Program.cs                       DI · JwtBearer · CSP+headers · políticas · Razor Components · seeder
appsettings.json                 ConnectionStrings · Jwt (Issuer/Audience/Key/TTLs)
Controllers/
  Public/
    AuthController.cs            POST login · POST refresh · POST logout · GET me
  Admin/
    UsuariosController.cs        [Authorize(Policy = AdminOnly)]
  Solicitante/
    ReservasController.cs        [Authorize(Policy = SolicitanteOnly)]
Components/
  _Imports.razor                 usings server-side (incluye RenderMode)
  App.razor                      shell HTML + <Routes @rendermode="InteractiveWebAssembly(prerender:false)">
  Routes.razor                   Router + AuthorizeRouteView (default layout = PublicLayout)
wwwroot/
  css/
    design-tokens.css            tokens + utilidades ed-* sobre Bootstrap
```

> La carpeta `Pages/` (Razor Pages legacy) **fue eliminada** durante la migración a Blazor + JWT.

## Cómo extender

### Añadir un endpoint API en un controlador existente

Para listar usuarios desde admin:

```csharp
// Controllers/Admin/UsuariosController.cs
[ApiController]
[Route("api/admin/usuarios")]
[Authorize(Policy = AppConstants.Policies.AdminOnly)]
public class UsuariosController(IUsuarioService usuarios) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var items = await usuarios.ListarAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearUsuarioRequestDto request)
    {
        var resp = await usuarios.CrearAsync(request);
        return resp.IsSuccess ? Ok(resp) : BadRequest(resp);
    }
}
```

El servicio (`IUsuarioService`) viene de `edificio_digital.Service`. El controlador no conoce EF ni Postgres.

### Añadir un controlador nuevo en una carpeta de rol existente

Por ejemplo reservas para solicitantes:

```csharp
// Controllers/Solicitante/ReservasController.cs
[ApiController]
[Route("api/solicitante/reservas")]
[Authorize(Policy = AppConstants.Policies.SolicitanteOnly)]
public class ReservasController(IReservaService reservas) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearReservaRequestDto request)
    {
        var solicitanteId = Guid.Parse(User.FindFirstValue(AppConstants.Claims.UserId)!);
        var resp = await reservas.CrearAsync(solicitanteId, request);
        return resp.IsSuccess ? Ok(resp) : BadRequest(resp);
    }
}
```

Luego desde el cliente Blazor (`edificio_digital.Client/Views/Solicitante/NuevaReserva.razor`):

```csharp
var resp = await Http.PostAsJsonAsync("api/solicitante/reservas", request);
```

### Añadir una nueva carpeta de rol (controlador + política)

1. Define el rol en `AppConstants.Roles` y la política en `AppConstants.Policies`.
2. Registra la política en `Program.cs`:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppConstants.Policies.NuevaPolicy,
        p => p.RequireRole(AppConstants.Roles.NuevoRol));
});
```

3. Crea `Controllers/<Rol>/<Recurso>Controller.cs` con `[Authorize(Policy = AppConstants.Policies.NuevaPolicy)]`.
4. En el cliente, crea la carpeta `Views/<Rol>/` con su `_Imports.razor`:

```razor
@layout NuevoLayout
@attribute [Authorize(Roles = AppConstants.Roles.NuevoRol)]
```

5. Crea el `Layouts/<NuevoLayout>.razor` en `.Client` (puedes copiar `AdminLayout.razor`).
6. Siembra el rol/usuario en `DbSeeder`.

### Cómo se monta Blazor (no tocar a la ligera)

`Program.cs` registra:

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// ...

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(edificio_digital.Client._Imports).Assembly);
```

`AddAdditionalAssemblies` es lo que descubre las páginas `@page` de `.Client`. Si se añade otro proyecto con páginas, hay que listarlo aquí también.

`Components/App.razor` usa `<Routes @rendermode="@(new InteractiveWebAssemblyRenderMode(prerender: false))" />` — **prerender desactivado** porque las páginas inyectan `HttpClient` (registrado solo en el DI del cliente WASM, no en el del server). Si lo reactivas, el server revienta al renderizar páginas WASM.

### Cómo funciona la autenticación JWT

```csharp
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
```

- `AuthController.Login` valida credenciales, llama a `IJwtTokenService` para generar access + refresh value, persiste el hash SHA-256 del refresh en `seguridad.refresh_tokens`, devuelve el access en JSON y el refresh en `Set-Cookie` (HttpOnly · SameSite=Strict · Path=`/api/auth`).
- `AuthController.Refresh` lee la cookie, valida hash en DB, rota (revoca el viejo + inserta nuevo), devuelve nuevo access + nuevo refresh en cookie.
- `AuthController.Logout` revoca el refresh en DB (`RevokedAt`) y borra la cookie. **Logout real**, no client-only.
- **Cambia `Jwt:Key`** en `appsettings.json` antes de cualquier deploy. El startup valida que tenga ≥32 caracteres y truena si no.

### Headers de seguridad y CSP

`Program.cs` agrega un middleware temprano que setea en cada respuesta:

```
Content-Security-Policy: default-src 'self';
                         script-src 'self' 'wasm-unsafe-eval' https://cdn.jsdelivr.net;
                         style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net https://fonts.googleapis.com;
                         font-src 'self' https://fonts.gstatic.com;
                         img-src 'self' data: blob:;
                         connect-src 'self';
                         frame-ancestors 'none';
                         base-uri 'self';
                         form-action 'self';
                         object-src 'none';
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Referrer-Policy: strict-origin-when-cross-origin
Permissions-Policy: camera=(), microphone=(), geolocation=(), interest-cohort=()
Cross-Origin-Opener-Policy: same-origin
Cross-Origin-Resource-Policy: same-origin
```

**Cuándo tocar la CSP**:

| Cambio | Directiva a actualizar |
|---|---|
| Cargar JS de otro CDN | `script-src` (jamás `'unsafe-inline'`) |
| Cargar CSS o fuentes nuevas | `style-src` / `font-src` |
| Llamar a otra API (analytics, mapa, etc.) | `connect-src` |
| Embebir imágenes externas | `img-src` |
| Embebir iframes | `frame-src` y reconsiderar `frame-ancestors` |

`'unsafe-inline'` está habilitado solo para **estilos** porque Bootstrap y los `style="..."` inline de componentes lo requieren. Para scripts es **prohibido** — eliminarlo cierra la mayor superficie de XSS.

Si Blazor falla a cargar tras un cambio, DevTools → Console muestra `Refused to load ... because it violates the following Content Security Policy directive`. Ese mensaje te dice exactamente qué ajustar.

## Diseño visual

Antes de tocar `App.razor`, `wwwroot/css/design-tokens.css` o cualquier markup, leer **`.claude/skills/edificio-digital-design/SKILL.md`**. Define paleta, tipografía, patrones reutilizables (`ed-eyebrow`, `ed-card-glass`, `ed-status`, `ed-bg-aurora`, `ed-h1-gradient`) y reglas (Bootstrap-first, no emoji, headings `fw-medium`, etc.).

## Qué evitar

- No referencies `AppDbContext` desde un controlador. Crea (o amplía) un servicio en `Service` y consúmelo.
- No copies cadenas como `"admin"`, `"/api/auth/login"`, `"AdminOnly"`. Todas viven en `AppConstants`.
- No mezcles `[Authorize]` con `[AllowAnonymous]` a nivel clase + nivel método contradiciéndose. El analizador `ASP0026` lo detecta. Si la mayoría del controlador es anónimo y un método requiere auth, marca cada método con `[AllowAnonymous]` o `[Authorize]` explícitamente, sin atributo de clase.
- No vuelvas a habilitar `MapRazorPages`. La UI vive en `.Client` y eso colisionaría rutas.
- No metas componentes Blazor de página dentro de `Components/` del server. Esa carpeta es solo el shell; las páginas viven en `.Client/Views/`.
