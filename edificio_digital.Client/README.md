# edificio_digital.Client

**Razor Class Library** con componentes y assets compartibles entre el host actual y futuros clientes (por ejemplo, una app Blazor WASM que consuma los endpoints `/api/...`).

## Responsabilidad

- Componentes Razor reutilizables (`*.razor`) y sus estilos co-localizados (`*.razor.css`).
- Interop con JavaScript (`ExampleJsInterop.cs`, `wwwroot/exampleJsInterop.js`) cuando se necesite tocar el navegador.
- Assets estáticos compartidos (`wwwroot/`) — se sirven desde el host con prefijo `_content/edificio_digital.Client/`.

## Reglas

- Solo depende de `Microsoft.AspNetCore.Components.Web`.
- **No referencia a `Service` ni a `Entity`.** Si un componente necesita datos, recíbelos por parámetros (`[Parameter]`) o consume una abstracción (interfaz declarada en `Models`) inyectada por DI.
- Apto para ejecutarse en navegador (`<SupportedPlatform Include="browser" />`), por eso evita APIs de servidor.

## Estructura actual

```
Component1.razor              componente de ejemplo
Component1.razor.css          estilos co-localizados (CSS isolation)
ExampleJsInterop.cs           clase de interop JS
_Imports.razor                using comunes
wwwroot/
  background.png
  exampleJsInterop.js
```

## Cómo extender

### Añadir un componente reutilizable

Para una tarjeta de reserva que se vea igual en `/Admin/Reservas` y `/Solicitante/Reservas`:

`ReservaCard.razor`:

```razor
@namespace edificio_digital.Client

<div class="reserva-card border rounded p-3 mb-2">
    <h6 class="mb-1">@Titulo</h6>
    <small class="text-muted">@Fecha.ToString("yyyy-MM-dd HH:mm")</small>
    <div class="mt-2">
        <span class="badge bg-secondary">@Estado</span>
    </div>
</div>

@code {
    [Parameter, EditorRequired] public string Titulo { get; set; } = default!;
    [Parameter, EditorRequired] public DateTime Fecha { get; set; }
    [Parameter, EditorRequired] public string Estado { get; set; } = default!;
}
```

Desde una Razor Page del host:

```cshtml
@addTagHelper *, edificio_digital.Client
<reserva-card titulo="Sala A" fecha="@DateTime.Now" estado="Pendiente" />
```

> Nota: en Razor Pages clásicas (no Blazor) el uso típico es vía `<component type="typeof(...)" />`. El proyecto Web ya referencia esta biblioteca para que esté lista cuando se incorporen componentes interactivos.

### Compartir assets estáticos

Coloca el archivo en `wwwroot/` de esta biblioteca y referencialo desde el host:

```html
<link rel="stylesheet" href="~/_content/edificio_digital.Client/site-shared.css" />
```

### Cuándo agregar un componente aquí vs. en el host

| Va aquí | Va en `edificio_digital/Pages/Shared` |
|---|---|
| Reutilizable por varios layouts o por un futuro cliente Blazor WASM | Es un fragmento atado a una página/layout específico del host |
| No depende de `HttpContext` ni servidor | Usa Tag Helpers de ASP.NET (`asp-page`, `asp-for`) |
| Recibe todo por `[Parameter]` | Lee `ViewData`, `User.Identity`, etc. |

## Qué evitar

- No referencies `edificio_digital.Service` ni `edificio_digital.Entity` — rompería la regla de portabilidad de la biblioteca.
- No uses `HttpContext` — esta biblioteca puede correr en un entorno donde no exista (Blazor WASM).
- Si el componente necesita datos del backend, defínelo recibiéndolos como parámetros y deja que el host los consiga.
