# edificio_digital.Entity

**Capa Infrastructure.** Implementa los contratos del dominio contra tecnología concreta: PostgreSQL via EF Core.

## Responsabilidad

- **Modelos EF Core** (`model/`) — clases mapeadas a tablas, con atributos `[Table]`, `[Column]`, navegaciones. Son la representación de persistencia, no la del dominio.
- **`Data/AppDbContext.cs`** — agrupa todos los `DbSet` y la configuración fluent.
- **Repositorios** (`Auth/`, ...) — implementan las interfaces declaradas en `edificio_digital.Models/Domain/`. Convierten entidades EF a entidades de dominio (`UserCredential`) y viceversa.
- **`Data/DbSeeder.cs`** — aplica migraciones y deja la base de datos lista al iniciar el host (idempotente).
- **`Migrations/`** — historial generado por `dotnet ef migrations add`.

## Reglas

- Solo `edificio_digital.Models` está referenciado desde el `.csproj`.
- Todo lo que la app necesita hacer contra Postgres pasa por aquí.
- Los modelos EF (`Entity.Model.Usuario`, etc.) **no salen de esta capa**. Si se necesita ese dato fuera, se mapea a un tipo del dominio (`Models.Domain.*`) o a un DTO (`Models.*Dto`).

## Estructura actual

```
Data/
  AppDbContext.cs          DbSets + OnModelCreating
  AppDbContextFactory.cs   factory para 'dotnet ef' en design-time
  DbSeeder.cs              Migrate + upsert de roles + admin/admin
Auth/
  BcryptPasswordHasher.cs              implementa IPasswordHasher
  PostgreSqlAuthRepository.cs          implementa IAuthRepository
  PostgreSqlRefreshTokenRepository.cs  implementa IRefreshTokenRepository
model/
  usuario/  estructura/  recursos/  seguridad/  reservas/  auditoria/
  seguridad/refresh-token.cs           tabla seguridad.refresh_tokens (JWT refresh)
Migrations/
  ...                                  generadas por EF Core
```

La tabla `seguridad.refresh_tokens` guarda el **hash SHA-256** del refresh token (no el valor en claro), con índice único por hash y por usuario. Permite logout server-side real (`RevokedAt`) y rotación con cadena de reemplazo (`ReplacedByTokenId`).

## Cómo extender

### Receta: añadir el repositorio de Reservas

**1. (Si la entidad no existe) modela la tabla en `model/reservas/reserva.cs`:**

```csharp
[Table("reservas", Schema = "reservas")]
public class Reserva
{
    [Key] public Guid Id { get; set; }
    [Column("ambiente_id")] public Guid AmbienteId { get; set; }
    [Column("solicitante_id")] public Guid SolicitanteId { get; set; }
    [Column("fecha_inicio")] public DateTime FechaInicio { get; set; }
    [Column("fecha_fin")] public DateTime FechaFin { get; set; }
    [Column("estado"), MaxLength(50)] public string Estado { get; set; } = "Pendiente";
    [Column("motivo"), MaxLength(500)] public string Motivo { get; set; } = string.Empty;
}
```

**2. Asegúrate de que `AppDbContext` exponga el `DbSet` y configure la entidad** (en este repo ya hay `DbSet<Reserva>` y configuración fluent para muchos casos).

**3. Implementa el repositorio en `Reservas/PostgreSqlReservaRepository.cs`:**

```csharp
using edificio_digital.Entity.Data;
using edificio_digital.Models.Domain.Reservas;
using EFReserva = edificio_digital.Entity.Model.Reserva;

namespace edificio_digital.Entity.Reservas;

public class PostgreSqlReservaRepository(AppDbContext db) : IReservaRepository
{
    public async Task<Reserva?> GetByIdAsync(Guid id) =>
        await db.Reservas
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(MapToDomain)
            .FirstOrDefaultAsync();

    public async Task<List<Reserva>> ListBySolicitanteAsync(Guid solicitanteId) =>
        await db.Reservas
            .AsNoTracking()
            .Where(x => x.SolicitanteId == solicitanteId)
            .Select(MapToDomain)
            .ToListAsync();

    public async Task AddAsync(Reserva reserva)
    {
        db.Reservas.Add(new EFReserva
        {
            Id = reserva.Id,
            AmbienteId = reserva.AmbienteId,
            SolicitanteId = reserva.SolicitanteId,
            FechaInicio = reserva.FechaInicio,
            FechaFin = reserva.FechaFin,
            Estado = reserva.Estado,
            Motivo = reserva.Motivo
        });
        await db.SaveChangesAsync();
    }

    private static readonly Expression<Func<EFReserva, Reserva>> MapToDomain = x => new Reserva
    {
        Id = x.Id,
        AmbienteId = x.AmbienteId,
        SolicitanteId = x.SolicitanteId,
        FechaInicio = x.FechaInicio,
        FechaFin = x.FechaFin,
        Estado = x.Estado,
        Motivo = x.Motivo
    };
}
```

**4. Registra en `Program.cs`:**

```csharp
builder.Services.AddScoped<IReservaRepository, PostgreSqlReservaRepository>();
```

### Crear una migración

Cuando cambies un modelo EF o el `OnModelCreating`:

```powershell
dotnet ef migrations add NombreDescriptivo `
  --project edificio_digital.Entity/edificio_digital.Entity.csproj `
  --startup-project edificio_digital/edificio_digital.csproj `
  --context AppDbContext
```

El `DbSeeder.SeedAsync` se ejecuta al arrancar la app y aplica `MigrateAsync` automáticamente, así que para entornos de desarrollo no hace falta `database update` manual.

### Extender el seeder

`DbSeeder` está pensado para ser **idempotente y seguro**. Para añadir más datos base:

```csharp
var rolDocente = await EnsureRolAsync(db, "docente", "Docente", ct);
await EnsureUsuarioConRolAsync(db,
    email: "docente@edificiodigital.com",
    nombreUsuario: "docente",
    nombreCompleto: "Docente de prueba",
    tipo: "Docente",
    contrasena: "docente",
    rol: rolDocente, ct);
```

Si la entidad ya existe, el seeder solo actualiza los campos sensibles (contraseña, tipo, activo). Mantén esa propiedad: nada de `INSERT` ciego.

## Qué evitar

- No filtres tipos EF (`Usuario`, `Reserva` de `Entity.Model`) hacia Application o Web. Mapea siempre al tipo del dominio.
- No coloques lógica de negocio en repositorios — solo lectura/escritura. La lógica vive en `Service`.
- No semilles datos importantes desde una migración con `migrationBuilder.InsertData(...)`. Usa el seeder, así puedes evolucionar los datos sin tocar el historial de migraciones.
