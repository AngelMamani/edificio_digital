# edificio_digital

Sistema en .NET para gestion de ambientes, recursos y reservas con arquitectura por capas y PostgreSQL.

## Arquitectura

- `edificio_digital`: host web (Razor Pages + endpoints API).
- `edificio_digital.Service`: logica de negocio y casos de uso.
- `edificio_digital.Entity`: persistencia (EF Core, `AppDbContext`, migraciones).
- `edificio_digital.Models`: DTOs y contratos de intercambio.
- `edificio_digital.Client`: biblioteca Razor para componentes compartidos.

## Base de datos

- Motor: PostgreSQL (Npgsql + EF Core).
- Modelo alineado al DER `PRACTICAS/der-edificio-digital.dbml`.
- Esquemas por modulo:
  - `usuario`
  - `estructura`
  - `recursos`
  - `seguridad`
  - `reservas`
  - `auditoria`

### Nota de auditoria JSON

Para PostgreSQL se usa `jsonb` en:
- `auditoria.bitacora_auditoria.valores_anteriores_json`
- `auditoria.bitacora_auditoria.valores_nuevos_json`
- `auditoria.historial_movimiento_global.detalle_json`

## Ejecutar en local

1. Configura `ConnectionStrings:PostgreSql` en `edificio_digital/appsettings.json`.
2. Aplica migraciones:

```bash
dotnet ef database update --project edificio_digital.Entity/edificio_digital.Entity.csproj --startup-project edificio_digital/edificio_digital.csproj --context AppDbContext
```

3. Ejecuta la app:

```bash
dotnet run --project edificio_digital/edificio_digital.csproj
```

## Autenticacion actual

- Login por correo y contrasena.
- Flujo disponible en:
  - `Pages/Login.cshtml`
  - `POST /api/auth/login`
- Estado actual: token de demostracion (sin JWT productivo aun).
