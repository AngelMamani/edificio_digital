---
name: edificio-digital-design
description: Sistema de diseño del frontend de Edificio Digital. Úsalo al crear o modificar vistas, layouts o componentes Blazor en edificio_digital.Client. Captura el lenguaje visual lila institucional adaptado desde plantill/edificio_digital y montado sobre Bootstrap 5.
---

# Edificio Digital — Sistema de diseño

Documento de referencia para producir UI consistente. Aplica a todo `edificio_digital.Client/`.

## Filosofía

Institucional, suave, baja elevación, conducido por la paleta lila. Geometría sobre decoración. **Bootstrap 5 es la base estructural**; el lenguaje visual (paleta, tipografía, patrones custom) se monta encima vía design tokens. No se re-implementa lo que Bootstrap ya resuelve (grid, formularios, modales, dropdowns, alerts, navbar, cards).

## Stack

- **Framework**: Bootstrap 5.3 vía CDN. Clases `.btn`, `.card`, `.navbar`, `.alert`, `.form-*`, grid `.container/.row/.col-*`, utilidades de espaciado/flex.
- **Capa custom**: `edificio_digital/wwwroot/css/design-tokens.css` (servida por el host). Sobreescribe variables CSS de Bootstrap e introduce clases `ed-*` para patrones que Bootstrap no cubre.
- **Componentes Blazor reutilizables**: `edificio_digital.Client/Components/`.

## Paleta

Origen: OKLCH (lila institucional). Mapeo a variables Bootstrap.

| Token | Valor OKLCH | Mapeo Bootstrap |
|---|---|---|
| `--ed-lila-100` | oklch(0.965 0.018 295) | `--bs-primary-bg-subtle` |
| `--ed-lila-300` | oklch(0.84 0.08 295) | acentos suaves |
| `--ed-lila-500` | oklch(0.58 0.16 295) | `--bs-primary` |
| `--ed-lila-600` | oklch(0.48 0.16 295) | hover de primary |
| `--ed-lila-700` | oklch(0.38 0.13 295) | `--bs-primary-text-emphasis`, links |
| `--ed-cyan-400` | oklch(0.78 0.13 220) | acento secundario |
| `--ed-violet-500` | oklch(0.55 0.20 305) | gradientes |
| `--ed-amber-400` | oklch(0.82 0.13 80) | warning |
| `--ed-rose-400` | oklch(0.72 0.16 18) | danger |
| `--ed-green-400` | oklch(0.78 0.14 155) | success |

**Neutros**: `--ed-paper` (fondo), `--ed-paper-2` (superficies elevadas), `--ed-line` (bordes), `--ed-ink` (texto principal), `--ed-ink-muted` (texto secundario). Úsalos en lugar de `text-muted/bg-light` cuando quieras el matiz institucional.

## Tipografía

- **Sans**: `Geist` — cuerpo, headings, UI.
- **Mono**: `Geist Mono` — eyebrows, metadatos, timestamps, refs.
- Pesos: headings `500` (NO `700`), cuerpo `400`, eyebrows `500`.
- Letter-spacing: headings `-0.025em`, cuerpo `-0.005em`, mono uppercase `0.08em a 0.12em`.
- Cuerpo: `15px / 1.55`.
- **Eyebrow pattern**: pequeño label en mono uppercase para etiquetar secciones o estados:
  ```html
  <span class="ed-eyebrow"><i class="dot"></i> EN VIVO</span>
  ```

## Layouts por rol

Cada layout vive en `edificio_digital.Client/Layouts/` y se selecciona desde el `_Imports.razor` de su carpeta de vistas.

| Layout | Ruta | Uso | Color navbar |
|---|---|---|---|
| `PublicLayout` | `/`, `/login`, `/access-denied` | Anónimo o pre-login | `bg-light` con borde inferior |
| `AdminLayout` | `/admin/**` | Rol `admin` | `bg-dark` (institucional) |
| `SolicitanteLayout` | `/solicitante/**` | Rol `solicitante` | `bg-primary-subtle` |

Layout = decisión de carpeta. Cada `Views/<Rol>/_Imports.razor` declara `@layout` para todas las páginas de ese rol.

## Patrones reutilizables

### Alert (`Components/Alert.razor`)
Envuelve Bootstrap `.alert-*`. Variantes: `success | info | warning | danger`. Soporta `Dismissible` y `Visible`.

### PageHeader (`Components/PageHeader.razor`)
Título + opcional eyebrow + slot de acciones. Usar arriba de toda página de Admin/Solicitante.

### Card / superficie
Bootstrap `.card` como base. Añadir `ed-card-glass` para variantes translúcidas (login sobre fondo con gradiente).

### Botones
- Acción principal: `.btn .btn-primary` (lila vía tokens).
- Acción secundaria: `.btn .btn-outline-primary` o `.btn-light`.
- Destructiva: `.btn-outline-danger`.
- Evitar `btn-success/.btn-warning` planos por saturación — preferir outlines fuera del primary.

### Status pills
Para estados (libre / próximo / ocupado):
```html
<span class="ed-status ed-status--free"><i></i> Libre</span>
<span class="ed-status ed-status--soon"><i></i> Próximo</span>
<span class="ed-status ed-status--busy"><i></i> Ocupado</span>
```

### Sombras
Usar tokens `--ed-shadow-1/2/3` en lugar de `box-shadow` arbitrario. Tinte lila preservado.

## Reglas

- **Bootstrap primero**. Antes de escribir CSS, comprobar si Bootstrap lo resuelve (espaciado `m-*/p-*`, flex `d-flex/gap-*`, grid `row/col-*`).
- **Iconografía**: solo SVG geométricos stroked (`stroke-width: 1.6`, `currentColor`, sin emoji).
- **No `<style>` inline en `.razor`**. Usar scoped `.razor.css` o extender `design-tokens.css`.
- **No vibrant blue** (Bootstrap default): siempre lila vía override de `--bs-primary`.
- Una sección no mezcla texto con gradiente y fondo con gradiente — uno u otro.
- Dark mode opcional vía `[data-theme="dark"]` en `<html>`. Definido en tokens; no inventar variables paralelas.

## Archivos clave

- Plantilla original (referencia visual, NO código fuente): `plantill/edificio_digital/`.
- Tokens y utilidades `ed-*`: `edificio_digital/wwwroot/css/design-tokens.css`.
- Componentes reutilizables: `edificio_digital.Client/Components/`.
- Layouts por rol: `edificio_digital.Client/Layouts/`.
- Vistas por rol: `edificio_digital.Client/Views/{Public,Admin,Solicitante}/`.

## Cuando crear un componente nuevo

Si vas a repetir la misma estructura de markup en ≥2 vistas, conviértelo en componente en `Components/`. Si la estructura es de página completa (header + body + footer), es un Layout, no un Component.
