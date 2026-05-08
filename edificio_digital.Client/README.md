# edificio_digital.Client

**Blazor WebAssembly** — la totalidad de la UI vive aquí. El servidor `edificio_digital` es API + shell (HTML root + render mode WASM) y delega todas las páginas a este proyecto.

> Si vienes a continuar el diseño de las páginas: lee primero el skill de diseño en `.claude/skills/edificio-digital-design/SKILL.md`. Define paleta, tipografía, layouts y patrones reutilizables.

## Stack

- **SDK**: `Microsoft.NET.Sdk.BlazorWebAssembly` (.NET 10).
- **Auth**: JWT (HS256). Access token en `localStorage`, refresh rotativo en cookie HttpOnly server-side. `AuthorizationMessageHandler` adjunta el Bearer y refresca automático ante 401.
- **Estilos**: Bootstrap 5 (CDN) + tokens custom en `edificio_digital/wwwroot/css/design-tokens.css`.
- **Tipografía**: Geist + Geist Mono (Google Fonts, cargadas en `App.razor` del server).

## Cómo levantar la app

Postgres tiene que estar corriendo y la cadena de conexión debe estar en `edificio_digital/appsettings.json` → `ConnectionStrings:PostgreSql`.

```powershell
dotnet run --project edificio_digital/edificio_digital.csproj
```

El server compila **y sirve** este proyecto WASM automáticamente — no hace falta `dotnet run` en `.Client`. Abre `http://localhost:5049`.

Usuario de prueba (creado por el seeder, idempotente):

| Email | Contraseña | Rol |
|---|---|---|
| `admin@edificiodigital.com` | `admin` | `admin` |

Para hot reload mientras editas `.razor`:

```powershell
dotnet watch --project edificio_digital/edificio_digital.csproj
```

## Estructura

```
edificio_digital.Client/
├─ Program.cs                 bootstrap WASM (HttpClient pipeline + AuthState)
├─ _Imports.razor             usings globales
├─ Auth/
│  ├─ TokenStorage.cs                       wrapper localStorage (key ed_access_token)
│  ├─ JwtAuthenticationStateProvider.cs     parsea JWT y expone ClaimsPrincipal
│  └─ AuthorizationMessageHandler.cs        DelegatingHandler: Bearer + refresh on 401
├─ Components/                reutilizables, no son páginas
│  ├─ Alert.razor
│  ├─ PageHeader.razor
│  └─ RedirectTo.razor
├─ Layouts/                   uno por rol
│  ├─ PublicLayout.razor
│  ├─ AdminLayout.razor
│  └─ SolicitanteLayout.razor
└─ Views/                     páginas con @page, agrupadas por rol
   ├─ Public/                 _Imports.razor → @layout PublicLayout
   │  ├─ Home.razor           @page "/"
   │  ├─ Login.razor          @page "/login"
   │  └─ AccessDenied.razor   @page "/access-denied"
   ├─ Admin/                  _Imports.razor → @layout AdminLayout + [Authorize(Roles=admin)]
   │  ├─ Index.razor          @page "/admin"
   │  └─ Usuarios.razor       @page "/admin/usuarios"
   └─ Solicitante/            _Imports.razor → @layout SolicitanteLayout + [Authorize(Roles=solicitante)]
      ├─ Index.razor          @page "/solicitante"
      └─ NuevaReserva.razor   @page "/solicitante/nueva-reserva"
```

**Convención clave**: el `_Imports.razor` de cada carpeta de `Views/<Rol>/` declara el `@layout` y el `[Authorize]` apropiados. Crear una página nueva en esa carpeta hereda ambas reglas automáticamente — no las repitas en la página.

## Cómo añadir una página nueva

Por ejemplo `/admin/reservas`:

1. Crea `Views/Admin/Reservas.razor`:

```razor
@page "/admin/reservas"

<PageTitle>Reservas · Admin</PageTitle>

<PageHeader Title="Reservas"
            Eyebrow="GESTIÓN"
            Subtitle="Listado de reservas en el sistema.">
    <Actions>
        <button class="btn btn-primary btn-sm">+ Nueva</button>
    </Actions>
</PageHeader>

<Alert Visible="@(!string.IsNullOrEmpty(error))" Variant="danger">@error</Alert>

@if (items is null)
{
    <div class="text-secondary small">Cargando…</div>
}
else if (items.Count == 0)
{
    <div class="text-secondary small">No hay reservas.</div>
}
else
{
    <div class="table-responsive">
        <table class="table align-middle">
            <thead>
                <tr><th>Ambiente</th><th>Solicitante</th><th>Fecha</th><th></th></tr>
            </thead>
            <tbody>
                @foreach (var r in items)
                {
                    <tr>
                        <td>@r.Ambiente</td>
                        <td>@r.Solicitante</td>
                        <td class="ed-mono">@r.Fecha.ToString("yyyy-MM-dd HH:mm")</td>
                        <td><span class="ed-status ed-status--free"><i></i> Activa</span></td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
}

@code {
    [Inject] private HttpClient Http { get; set; } = default!;

    private List<ReservaListadoDto>? items;
    private string error = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            items = await Http.GetFromJsonAsync<List<ReservaListadoDto>>("api/admin/reservas");
        }
        catch (Exception ex)
        {
            error = "No se pudieron cargar las reservas.";
        }
    }
}
```

2. Añade el link al navbar correspondiente (`Layouts/AdminLayout.razor`):

```razor
<li class="nav-item"><NavLink class="nav-link" href="/admin/reservas">Reservas</NavLink></li>
```

No tienes que tocar `_Imports.razor` ni declarar `[Authorize]` — la carpeta lo aporta.

## Cómo añadir un componente reutilizable

Si vas a repetir markup en ≥2 páginas, conviértelo en componente en `Components/`:

```razor
@* Components/StatCard.razor *@
<div class="card h-100">
    <div class="card-body">
        @if (!string.IsNullOrEmpty(Eyebrow))
        {
            <span class="ed-eyebrow d-inline-flex mb-2"><i class="dot"></i> @Eyebrow</span>
        }
        <div class="display-6 fw-medium" style="letter-spacing:-0.025em;">@Value</div>
        <p class="text-secondary small mb-0 mt-1">@Label</p>
    </div>
</div>

@code {
    [Parameter, EditorRequired] public string Value { get; set; } = "0";
    [Parameter, EditorRequired] public string Label { get; set; } = string.Empty;
    [Parameter] public string? Eyebrow { get; set; }
}
```

Está disponible automáticamente en cualquier `.razor` (gracias a `@using edificio_digital.Client.Components` en `_Imports.razor`):

```razor
<StatCard Value="42" Label="Reservas activas" Eyebrow="HOY" />
```

## Cómo llamar al backend

Inyecta `HttpClient` y llama a las rutas `/api/...`. El `AuthorizationMessageHandler` (registrado en el pipeline) adjunta el `Authorization: Bearer <access>` automáticamente y maneja el refresh ante 401:

```razor
@inject HttpClient Http

@code {
    private async Task Cargar()
    {
        var data = await Http.GetFromJsonAsync<List<UsuarioDto>>("api/admin/usuarios");
    }
}
```

Las rutas viven en `edificio_digital/Controllers/{Public,Admin,Solicitante}/*Controller.cs` del server. Cada controller es delgado: recibe DTO, llama al servicio, devuelve resultado. La autorización está a nivel de clase con `[Authorize(Policy = ...)]`.

**No** debes leer ni escribir el access token manualmente desde un componente. El handler se encarga. Si recibes un 401 después del refresh, significa que la sesión expiró sin posibilidad de renovación — el handler ya limpió el storage y notificó `AuthState.NotifySignedOut`, por lo que `<AuthorizeRouteView>` redirigirá a `/login` en la próxima navegación.

## Auth — cómo funciona

```
LOGIN
1. /login → POST /api/auth/login con { email, password }
2. Server responde { AccessToken, AccessTokenExpiresAt, ... } y Set-Cookie ed_refresh
3. Login.razor → TokenStorage.SetAccessTokenAsync(AccessToken)
4. JwtAuthenticationStateProvider.NotifyUserChanged() → parsea claims del JWT
5. <AuthorizeView Roles="admin">, AuthorizeRouteView usan el ClaimsPrincipal

LLAMADA A API
1. HttpClient envía request → AuthorizationMessageHandler
2. Handler lee TokenStorage, adjunta Authorization: Bearer <access>
3. Si server responde 401 → handler hace POST /api/auth/refresh (cookie auto)
4. Si refresh OK → guarda nuevo access, NotifyUserChanged, reintenta request original
5. Si refresh falla → RemoveAccessTokenAsync, NotifySignedOut, propaga 401

LOGOUT
1. Layout llama POST /api/auth/logout
2. Server marca el refresh como RevokedAt en DB y borra cookie ed_refresh
3. TokenStorage.RemoveAccessTokenAsync()
4. AuthState.NotifySignedOut()
5. NavigateTo /login
```

El parsing del JWT lo hace `JwtAuthenticationStateProvider` localmente (sin round-trip al server). Lee `exp`, `role`/`roles`, `email`, `sub`, etc., y mapea a `ClaimTypes.*` para que `AuthorizeView Roles="..."` funcione.

## Diseño y estilos

**Lee `.claude/skills/edificio-digital-design/SKILL.md` antes de tocar UI.** Resumen:

- Bootstrap 5 es la base. Antes de escribir CSS, comprueba si Bootstrap lo resuelve (`d-flex`, `gap-*`, `row/col-*`).
- Paleta lila vía `--bs-primary` ya sobreescrito. Nunca uses azul Bootstrap default.
- Patrones custom prefijados con `ed-*`: `ed-eyebrow`, `ed-card-glass`, `ed-bg-aurora`, `ed-status`, `ed-h1-gradient`, `ed-mono`.
- Headings con `fw-medium` (500), no `fw-bold`. Letter-spacing tight (`-0.025em`).
- Iconos: SVG geométricos stroked (`stroke-width: 1.6`, `currentColor`). Sin emoji.
- No metas `<style>` inline en `.razor` — usa scoped `.razor.css` o extiende `wwwroot/css/design-tokens.css` del server.

## Reglas de capa

- `.Client` solo depende de `Models` (DTOs y constantes). **No referenciar `Service` ni `Entity`** — esas viven server-side.
- Toda llamada al backend pasa por `HttpClient` contra `/api/...`.
- Todas las rutas, roles, políticas y claims se leen de `AppConstants` (en `edificio_digital.Models.Common`). Nunca hardcodees `"admin"`, `"/api/auth/login"`, etc.

## XSS — qué no hacer en componentes

El access token vive en `localStorage`, así que cualquier script que se ejecute en la app lo lee. **Cualquier XSS = sesión robada.** El CSP (configurado en el server) bloquea scripts inline y de orígenes ajenos, pero la primera línea de defensa es no abrir vectores en código:

| ❌ Inseguro | ✅ Seguro |
|---|---|
| `@((MarkupString)userInput)` | `@userInput` (Blazor escapa por defecto) |
| `<a href="@url">` con `url` de input externo sin validar | Validar que `url` empiece con `/`, `https://` o `http://`; rechazar `javascript:` |
| `<img src="@imgUrl">` con URL del usuario | Mismo: allowlist de esquemas |
| `Html.Raw(...)` o concatenar strings con HTML | Componentes con `RenderFragment` y `[Parameter] string Texto` |
| Inyectar `<script>` desde data del backend | Datos van en variables, nunca como markup |
| `IJSRuntime.InvokeVoidAsync("eval", ...)` | Funciones JS predefinidas con argumentos tipados |

Reglas concretas:

1. **Nunca uses `MarkupString`** salvo con contenido **literal** del propio código (constantes, recursos compilados). Si necesitas formatear (negrita, cursiva), usa componentes Razor con `RenderFragment`.
2. **URLs dinámicas**: si la URL viene de input del usuario o de un campo de la BD que el usuario edita, valida antes:
   ```csharp
   private static string SafeUrl(string? url) =>
       string.IsNullOrEmpty(url) ? "#" :
       (url.StartsWith("/") || url.StartsWith("https://") || url.StartsWith("http://")) ? url :
       "#";
   ```
3. **No metas `<script>` ni `<style>` dentro de `.razor` desde valores dinámicos.** Usa `wwwroot/css/design-tokens.css` para estilos globales.
4. **Atributos de evento**: nunca `onclick="@stringFromUser"`. Usa el binding tipado: `@onclick="HandleClick"`.
5. **Iframes**: si necesitas embebir un iframe, primero suma el origen permitido a `frame-src` en CSP (server `Program.cs`) y considera reactivar `frame-ancestors` correctamente.
6. **Antes de subir un PR**, busca `MarkupString` y `Html.Raw` en tu diff. Si aparecen, debes justificarlos.

El refresh token está en cookie HttpOnly — no se puede leer ni siquiera con XSS. Y el access token expira en 15 min. Aún así: **un XSS sigue siendo crítico** porque el atacante puede hacer requests con el access mientras dure la sesión.

## Cosas que evitar

- No agregues `<NavLink>` a páginas que aún no existen — Blazor renderiza el link pero el route no resuelve.
- No mezcles `[Authorize]` por página y por carpeta. La carpeta gana — déjalo en `_Imports.razor`.
- No uses `forceLoad: true` en `NavigationManager.NavigateTo` salvo después de logout — rompe el estado WASM.
- No referencias `HttpContext` ni APIs server. Si el componente necesita algo del servidor, pide un endpoint.
- No metas archivos de página fuera de `Views/<Rol>/` — el `@layout` no se hereda y termina sin layout.

## Pendientes conocidos

- `Component1.razor` y `ExampleJsInterop.cs` quedaron del template inicial. Eliminables.
- Páginas `Admin/Usuarios.razor`, `Solicitante/Index.razor`, `Solicitante/NuevaReserva.razor` son esqueletos — falta lógica de listado y formularios.
- `AppConstants.Pages` aún apunta a rutas Razor legacy (`/Login`, `/Admin/Index`); las rutas Blazor son lowercase. Considerar agregar `AppConstants.BlazorRoutes` o renombrar.
- El handler de auth no serializa el refresh: si varias requests reciben 401 al mismo tiempo, dispararían múltiples `/refresh` concurrentes (la rotación los invalidaría entre sí). Para tráfico real, agregar un `SemaphoreSlim` que asegure un único refresh activo a la vez.
