# edificio_digital.Models

**Capa Domain + Shared Kernel.** Es el centro de Clean Architecture: nadie depende de nada externo y todos dependen de él.

## Responsabilidad

Esta biblioteca contiene únicamente:

- **Entidades de dominio** (`Domain/`) — POCOs que modelan reglas de negocio. Sin atributos de EF, sin nada que ate al transporte.
- **Contratos de repositorio** (`Domain/.../I*Repository.cs`) — interfaces que la capa Application requiere y que Infrastructure implementa.
- **DTOs** (`Auth/`, etc.) — contratos de intercambio entre capas y transportes (controladores HTTP, cliente Blazor WASM, integraciones futuras).
- **`Common/AppConstants.cs`** — única fuente de verdad para rutas API, roles, claims, políticas y esquema de cookie. La consumen el host (controladores), Application, el cliente Blazor y cualquier consumidor externo.

## Reglas

- **No referencia a ningún otro proyecto de la solución.** El `.csproj` no tiene `<ProjectReference>`.
- No usa Entity Framework, ASP.NET, ni librerías de infraestructura.
- Si una clase necesita conocer la base de datos, no pertenece aquí.

## Estructura actual

```
Auth/
  LoginRequestDto.cs           (POST /api/auth/login)
  LoginResponseDto.cs          (UserId, Email, NombreCompleto, Roles, AccessToken, AccessTokenExpiresAt)
  AuthLoginResultDto.cs        (wrapper interno: response + refresh token + expiry — para el controller)
  RefreshTokenResultDto.cs     (response de POST /api/auth/refresh)
Common/
  AppConstants.cs              (rutas API, roles, claims, políticas, layouts legacy)
  JwtSettings.cs               (Issuer, Audience, Key, TTLs, refresh cookie name/path)
Domain/
  Auth/
    UserCredential.cs          (entidad de dominio)
    RefreshToken.cs            (entidad de dominio: refresh token persistido)
    IAuthRepository.cs         (contrato; GetByEmailAsync + GetByIdAsync)
    IRefreshTokenRepository.cs (contrato; Add/FindByHash/Update/RevokeAll)
    IPasswordHasher.cs         (contrato implementado por Infrastructure)
```

## Cómo extender

### 1. Añadir un nuevo dominio (ej. Reservas)

Crea `Domain/Reservas/Reserva.cs` con la entidad pura:

```csharp
namespace edificio_digital.Models.Domain.Reservas;

public class Reserva
{
    public Guid Id { get; set; }
    public Guid AmbienteId { get; set; }
    public Guid SolicitanteId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public string Motivo { get; set; } = string.Empty;
}
```

Y su contrato de repositorio en `Domain/Reservas/IReservaRepository.cs`:

```csharp
namespace edificio_digital.Models.Domain.Reservas;

public interface IReservaRepository
{
    Task<Reserva?> GetByIdAsync(Guid id);
    Task<List<Reserva>> ListBySolicitanteAsync(Guid solicitanteId);
    Task AddAsync(Reserva reserva);
}
```

### 2. Añadir DTOs de transporte

Para el formulario o endpoint correspondiente, en `Reservas/`:

```csharp
namespace edificio_digital.Models.Reservas;

public class CrearReservaRequestDto
{
    [Required] public Guid AmbienteId { get; set; }
    [Required] public DateTime FechaInicio { get; set; }
    [Required] public DateTime FechaFin { get; set; }
    [Required, MaxLength(500)] public string Motivo { get; set; } = string.Empty;
}
```

### 3. Añadir constantes compartidas

Si la nueva funcionalidad expone rutas API, agrégalas dentro de `AppConstants.ApiRoutes`. Si necesitas referenciar una ruta Blazor desde el server (ej. en `LoginPath`/`AccessDeniedPath`), agrégala bajo una sección dedicada (`BlazorRoutes` o similar).

```csharp
public static class ApiRoutes
{
    public static class Reservas
    {
        public const string Group = ApiBase + "/solicitante/reservas";
        public const string Crear = Group;
        public const string Listar = Group;
        public const string Detalle = Group + "/{id:guid}";
    }
}
```

Cualquier capa (host, Application, cliente Blazor, integraciones) puede importar y usar estas constantes sin duplicar literales.

> **Nota sobre `AppConstants.Pages`**: las constantes existentes (`/Login`, `/Admin/Index`, etc.) son rutas **Razor Pages legacy** que ya no se rutean. Las rutas Blazor reales son lowercase (`/login`, `/admin`, `/admin/usuarios`, `/solicitante`, `/solicitante/nueva-reserva`). Pendiente: alinear estas constantes a las rutas Blazor o introducir `AppConstants.BlazorRoutes` y deprecar `Pages`.

## Qué evitar

- No declares aquí clases que necesiten `[Table]`, `[Column]` ni anotaciones de EF — eso vive en `edificio_digital.Entity/Model/...`.
- No expongas tipos de ASP.NET (`HttpContext`, `IActionResult`).
- Si dudas si algo va aquí: pregúntate "¿esto sigue teniendo sentido si cambiamos la base de datos o el framework web?". Si sí, va aquí.
