# edificio_digital.Models

**Capa Domain + Shared Kernel.** Es el centro de Clean Architecture: nadie depende de nada externo y todos dependen de él.

## Responsabilidad

Esta biblioteca contiene únicamente:

- **Entidades de dominio** (`Domain/`) — POCOs que modelan reglas de negocio. Sin atributos de EF, sin nada que ate al transporte.
- **Contratos de repositorio** (`Domain/.../I*Repository.cs`) — interfaces que la capa Application requiere y que Infrastructure implementa.
- **DTOs** (`Auth/`, etc.) — contratos de intercambio entre capas y transportes (Razor Pages, API, futuros clientes).
- **`Common/AppConstants.cs`** — única fuente de verdad para rutas, layouts, roles, claims, políticas y esquema de cookie. La consumen Web, Service y cualquier consumidor externo.

## Reglas

- **No referencia a ningún otro proyecto de la solución.** El `.csproj` no tiene `<ProjectReference>`.
- No usa Entity Framework, ASP.NET, ni librerías de infraestructura.
- Si una clase necesita conocer la base de datos, no pertenece aquí.

## Estructura actual

```
Auth/
  LoginRequestDto.cs       (POST /Login y POST /api/auth/login)
  LoginResponseDto.cs      (rol primario, roles, redirección destino)
Common/
  AppConstants.cs          (rutas, layouts, roles, claims, políticas, cookie)
Domain/
  Auth/
    UserCredential.cs      (entidad de dominio)
    IAuthRepository.cs     (contrato implementado por Infrastructure)
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

Si la nueva funcionalidad expone rutas, agrégalas dentro de `AppConstants`:

```csharp
public static class ApiRoutes
{
    public static class Reservas
    {
        public const string Group = ApiBase + "/reservas";
        public const string Crear = Group;
        public const string Listar = Group;
        public const string Detalle = Group + "/{id:guid}";
    }
}

public static class Pages
{
    public const string ReservasIndex = "/Solicitante/Reservas/Index";
    public const string NuevaReserva  = "/Solicitante/Reservas/Nueva";
}
```

Cualquier capa (Web, Service, futuros clientes) puede importar y usar estas constantes sin duplicar literales.

## Qué evitar

- No declares aquí clases que necesiten `[Table]`, `[Column]` ni anotaciones de EF — eso vive en `edificio_digital.Entity/Model/...`.
- No expongas tipos de ASP.NET (`HttpContext`, `IActionResult`).
- Si dudas si algo va aquí: pregúntate "¿esto sigue teniendo sentido si cambiamos la base de datos o el framework web?". Si sí, va aquí.
