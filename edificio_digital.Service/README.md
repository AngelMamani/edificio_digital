# edificio_digital.Service

**Capa Application.** Orquesta los casos de uso del sistema. Es el lugar donde vive la lógica de negocio que no es puramente del dominio.

## Responsabilidad

- Implementa **casos de uso** (verbos del sistema): `LoginAsync`, `CrearReservaAsync`, `AprobarReservaAsync`, etc.
- Coordina entidades del dominio, repositorios (a través de sus interfaces) y servicios externos.
- Devuelve DTOs definidos en `Models`. **Nunca expone tipos de EF Core, HttpContext ni cookies.**
- Aplica reglas que dependen de varias entidades o de operaciones combinadas (validar disponibilidad, calcular prioridad, decidir página destino tras login, etc.).

## Reglas

- Solo referencia a `edificio_digital.Models`.
- No conoce EF Core, no conoce ASP.NET, no instancia repositorios concretos.
- Recibe dependencias por constructor (`IAuthRepository`, etc.). El contenedor DI se encarga de proveer la implementación.
- Cada servicio expone una interfaz (`IFooService`) y una implementación (`FooService`). El host las registra como `Scoped`.

## Estructura actual

```
Auth/
  IAuthService.cs
  AuthService.cs        valida credenciales, calcula rol primario, decide redirección
```

## Cómo extender

### Receta: añadir un caso de uso completo (ejemplo: crear reserva)

**1. Define el contrato en `Reservas/IReservaService.cs`:**

```csharp
using edificio_digital.Models.Reservas;

namespace edificio_digital.Service.Reservas;

public interface IReservaService
{
    Task<CrearReservaResponseDto> CrearAsync(Guid solicitanteId, CrearReservaRequestDto request);
}
```

**2. Implementa el caso de uso en `Reservas/ReservaService.cs`:**

```csharp
using edificio_digital.Models.Domain.Reservas;
using edificio_digital.Models.Reservas;

namespace edificio_digital.Service.Reservas;

public class ReservaService(IReservaRepository reservas) : IReservaService
{
    public async Task<CrearReservaResponseDto> CrearAsync(Guid solicitanteId, CrearReservaRequestDto request)
    {
        if (request.FechaFin <= request.FechaInicio)
        {
            return new CrearReservaResponseDto
            {
                IsSuccess = false,
                Message = "La fecha fin debe ser mayor a la fecha inicio."
            };
        }

        var reserva = new Reserva
        {
            Id = Guid.NewGuid(),
            AmbienteId = request.AmbienteId,
            SolicitanteId = solicitanteId,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            Motivo = request.Motivo,
            Estado = "Pendiente"
        };

        await reservas.AddAsync(reserva);

        return new CrearReservaResponseDto
        {
            IsSuccess = true,
            ReservaId = reserva.Id,
            Message = "Reserva creada en estado Pendiente."
        };
    }
}
```

**3. Registra en `Program.cs` (proyecto Web):**

```csharp
builder.Services.AddScoped<IReservaService, ReservaService>();
```

A partir de ahí cualquier `PageModel` o endpoint mínimo puede inyectar `IReservaService`. La capa Application no conoce HTTP — puede ser invocada igual desde una página Razor (in-process) que desde un endpoint API.

### Patrones útiles

- **Devuelve siempre DTOs**, no entidades de dominio. Si el caller necesita más datos, exponlos en el DTO.
- **No lances excepciones para errores de negocio esperados.** Devuelve un DTO con `IsSuccess = false` y `Message`. Reserva las excepciones para fallos técnicos (timeout, bug interno).
- **Un servicio por agregado.** Para módulos grandes (reservas, ambientes), divide en varios servicios pequeños en lugar de un `BigService` monolítico.
- **Las dependencias siempre son interfaces** definidas en `Models`. Si necesitas tiempo, sistema de archivos, mailer, etc., declara una interfaz en `Models` (ej. `IClock`, `IEmailSender`) y deja que Infrastructure las implemente.

## Qué evitar

- No instancies `AppDbContext`, no uses `DbContextOptions`. Solo Infrastructure conoce EF.
- No leas `HttpContext`, no firmes cookies, no devuelvas `IActionResult`. Eso es trabajo de la capa Web.
- No referencies `edificio_digital.Entity`. Si te ves tentado a hacerlo, te falta una interfaz en `Models`.
