// app.jsx — Edificio Digital homepage
const { useState, useEffect, useMemo, useRef } = React;

// ──────────────────────────────────────────────────────────────────
// data
// ──────────────────────────────────────────────────────────────────

const NAV_ITEMS = [
  { id: "inicio", label: "Inicio" },
  { id: "ambientes", label: "Ambientes" },
  { id: "disponibilidad", label: "Disponibilidad" },
  { id: "calendario", label: "Calendario" },
  { id: "contacto", label: "Contacto" },
];

const FEATURES = [
  { code: "01", title: "Reserva en segundos", tag: "RESERVA",     iconKey: "Calendar",
    desc: "Selecciona ambiente, hora y equipamiento. Confirmación inmediata con notificación al correo institucional." },
  { code: "02", title: "Calendario en tiempo real", tag: "AGENDA", iconKey: "Clock",
    desc: "Disponibilidad sincronizada cada minuto. Vista por día, semana o por sala con filtros por edificio." },
  { code: "03", title: "Gestión de equipamiento", tag: "INVENTARIO", iconKey: "Box",
    desc: "Inventario por ambiente: proyectores, pizarras, audio, mobiliario. Reporta incidencias en un toque." },
  { code: "04", title: "Consulta pública", tag: "PÚBLICO", iconKey: "Globe",
    desc: "Página abierta para visitantes y comunidad: horarios, planos y disponibilidad sin requerir cuenta." },
  { code: "05", title: "Panel administrativo", tag: "ADMIN", iconKey: "Shield",
    desc: "Roles, aprobaciones, bloqueos por mantenimiento y registro completo de auditoría por reserva." },
  { code: "06", title: "Reportes y estadísticas", tag: "DATOS", iconKey: "Chart",
    desc: "Tasa de ocupación por sala, picos de demanda, ranking de equipamiento y exportación a CSV/Excel." },
];

const ROOMS = [
  {
    ref: "A-204",
    name: "Sala de Reuniones Norte",
    type: "Reunión ejecutiva",
    capacity: 12,
    floor: "Piso 2",
    chips: ["Pantalla 65\"", "Videollamada", "Pizarra"],
    status: "free",
    next: "Libre hasta 14:30",
  },
  {
    ref: "B-001",
    name: "Auditorio Central",
    type: "Conferencias y eventos",
    capacity: 220,
    floor: "Planta baja",
    chips: ["Audio profesional", "Proyector 4K", "Streaming"],
    status: "soon",
    next: "Ocupado a las 15:00",
  },
  {
    ref: "L-312",
    name: "Laboratorio de Cómputo",
    type: "Práctica académica",
    capacity: 32,
    floor: "Piso 3",
    chips: ["32 estaciones", "GPU dedicada", "Aislante"],
    status: "busy",
    next: "Disponible 16:30",
  },
  {
    ref: "C-110",
    name: "Coworking Flex",
    type: "Espacio abierto",
    capacity: 48,
    floor: "Piso 1",
    chips: ["Hot desks", "Cabinas privadas", "Café"],
    status: "free",
    next: "Libre todo el día",
  },
];

const BENEFITS = [
  { num: "01", title: "Cero conflictos de reserva", desc: "Bloqueos atómicos y validación por servidor evitan choques de horario." },
  { num: "02", title: "Cumple política institucional", desc: "Aprobaciones jerárquicas y registro auditable para áreas sensibles." },
  { num: "03", title: "Integraciones nativas", desc: "Sincroniza con Google Calendar, Outlook y SSO institucional vía SAML." },
];

// Calendar events: room × day × {start,end} in 30-min slots
// Days: Lun=0..Vie=4, Times: 08:00..18:00 → 20 slots de 30min
const CAL_EVENTS = [
  { room: 0, day: 0, start: 1, end: 4, title: "Reunión Decanato", who: "Dra. Salgado", color: "lila" },
  { room: 0, day: 1, start: 6, end: 8, title: "Comité Académico", who: "Sec. Académica", color: "violet" },
  { room: 0, day: 2, start: 2, end: 5, title: "Inducción Docentes", who: "RR. HH.", color: "cyan" },
  { room: 0, day: 4, start: 0, end: 3, title: "Reunión Investigación", who: "Lab. Datos", color: "lila" },
  { room: 1, day: 0, start: 4, end: 9, title: "Conferencia IA Aplicada", who: "Vicerrectoría", color: "violet" },
  { room: 1, day: 2, start: 8, end: 14, title: "Foro Internacional", who: "Coop. Externa", color: "cyan" },
  { room: 1, day: 3, start: 5, end: 10, title: "Defensa de Tesis", who: "Posgrado", color: "amber" },
  { room: 2, day: 0, start: 0, end: 4, title: "Lab. Programación", who: "Prof. Núñez", color: "lila" },
  { room: 2, day: 1, start: 4, end: 8, title: "Lab. Redes", who: "Prof. Soto", color: "cyan" },
  { room: 2, day: 1, start: 12, end: 16, title: "Mantenimiento", who: "Soporte TI", color: "ghost" },
  { room: 2, day: 3, start: 2, end: 6, title: "Lab. ML", who: "Prof. Chávez", color: "violet" },
  { room: 2, day: 4, start: 8, end: 12, title: "Olimpiada Cómputo", who: "Estudiantes", color: "amber" },
  { room: 3, day: 0, start: 0, end: 20, title: "Coworking abierto", who: "Comunidad", color: "ghost" },
  { room: 3, day: 1, start: 0, end: 20, title: "Coworking abierto", who: "Comunidad", color: "ghost" },
  { room: 3, day: 2, start: 0, end: 20, title: "Coworking abierto", who: "Comunidad", color: "ghost" },
];

// ──────────────────────────────────────────────────────────────────
// inline SVG icons (stroked, geometric — no emoji, no illustrations)
// ──────────────────────────────────────────────────────────────────

const sw = { fill: "none", stroke: "currentColor", strokeWidth: 1.6, strokeLinecap: "round", strokeLinejoin: "round" };

const Icon = {
  Calendar: () => (
    <svg width="20" height="20" viewBox="0 0 24 24" {...sw}>
      <rect x="3.5" y="5" width="17" height="15" rx="2.5" />
      <path d="M3.5 9.5h17M8 3v4M16 3v4" />
      <circle cx="8" cy="13.5" r="0.8" fill="currentColor" stroke="none" />
      <circle cx="12" cy="13.5" r="0.8" fill="currentColor" stroke="none" />
      <circle cx="16" cy="13.5" r="0.8" fill="currentColor" stroke="none" />
    </svg>
  ),
  Clock: () => (
    <svg width="20" height="20" viewBox="0 0 24 24" {...sw}>
      <circle cx="12" cy="12" r="8.5" />
      <path d="M12 7.5V12l3 2" />
    </svg>
  ),
  Box: () => (
    <svg width="20" height="20" viewBox="0 0 24 24" {...sw}>
      <path d="M3.5 7.5L12 3l8.5 4.5v9L12 21 3.5 16.5v-9z" />
      <path d="M3.5 7.5L12 12l8.5-4.5M12 12v9" />
    </svg>
  ),
  Globe: () => (
    <svg width="20" height="20" viewBox="0 0 24 24" {...sw}>
      <circle cx="12" cy="12" r="8.5" />
      <path d="M3.5 12h17M12 3.5c2.5 3 2.5 14 0 17M12 3.5c-2.5 3-2.5 14 0 17" />
    </svg>
  ),
  Shield: () => (
    <svg width="20" height="20" viewBox="0 0 24 24" {...sw}>
      <path d="M12 3.5l7 2.5v6c0 4.5-3 7.5-7 9-4-1.5-7-4.5-7-9v-6l7-2.5z" />
      <path d="M9 12.5l2 2 4-4" />
    </svg>
  ),
  Chart: () => (
    <svg width="20" height="20" viewBox="0 0 24 24" {...sw}>
      <path d="M4 20h16M6 16V11M10 16V8M14 16v-6M18 16V6" />
    </svg>
  ),
  Arrow: () => (
    <svg width="14" height="14" viewBox="0 0 16 16" {...sw}><path d="M3 8h10M9 4l4 4-4 4" /></svg>
  ),
  Search: () => (
    <svg width="14" height="14" viewBox="0 0 16 16" {...sw}><circle cx="7" cy="7" r="4.5" /><path d="M10.5 10.5L14 14" /></svg>
  ),
  Users: () => (
    <svg width="14" height="14" viewBox="0 0 16 16" {...sw}>
      <circle cx="6" cy="6" r="2.5" />
      <path d="M2 14c0-2.2 1.8-4 4-4s4 1.8 4 4" />
      <circle cx="11.5" cy="6.5" r="2" />
      <path d="M14 13.5c0-1.6-1.2-3-2.7-3.4" />
    </svg>
  ),
  Pin: () => (
    <svg width="14" height="14" viewBox="0 0 16 16" {...sw}>
      <path d="M8 14s5-4.5 5-8.5a5 5 0 1 0-10 0C3 9.5 8 14 8 14z" />
      <circle cx="8" cy="5.5" r="1.8" />
    </svg>
  ),
  Check: () => (
    <svg width="14" height="14" viewBox="0 0 16 16" {...sw}><path d="M3 8.5l3 3 7-7" /></svg>
  ),
  Plus: () => (
    <svg width="14" height="14" viewBox="0 0 16 16" {...sw}><path d="M8 3v10M3 8h10" /></svg>
  ),
  ChevL: () => (
    <svg width="12" height="12" viewBox="0 0 16 16" {...sw}><path d="M10 3l-5 5 5 5" /></svg>
  ),
  ChevR: () => (
    <svg width="12" height="12" viewBox="0 0 16 16" {...sw}><path d="M6 3l5 5-5 5" /></svg>
  ),
  X: () => (
    <svg width="14" height="14" viewBox="0 0 16 16" {...sw}><path d="M3.5 3.5l9 9M12.5 3.5l-9 9" /></svg>
  ),
};

// ──────────────────────────────────────────────────────────────────
// Navbar
// ──────────────────────────────────────────────────────────────────

function Navbar() {
  const [active, setActive] = useState("inicio");
  return (
    <div className="nav-wrap">
      <nav className="nav">
        <a className="nav__brand" href="#">
          <span className="nav__mark" aria-hidden="true"></span>
          <b>Edificio Digital</b>
          <small>· v2.4</small>
        </a>
        <div className="nav__menu">
          {NAV_ITEMS.map((it) => (
            <a
              key={it.id}
              href={`#${it.id}`}
              className={active === it.id ? "is-active" : ""}
              onClick={(e) => { e.preventDefault(); setActive(it.id); document.getElementById(it.id)?.scrollIntoView({behavior: "smooth", block: "start"}); }}
            >
              {it.label}
            </a>
          ))}
        </div>
        <div className="nav__cta">
          <button className="btn btn--sm">Iniciar sesión</button>
          <button className="btn btn--sm btn--lila">
            Reservar ambiente <Icon.Arrow />
          </button>
        </div>
      </nav>
    </div>
  );
}

// ──────────────────────────────────────────────────────────────────
// Hero with dashboard mock
// ──────────────────────────────────────────────────────────────────

function Hero() {
  return (
    <section className="hero shell" id="inicio">
      <div className="hero__grid">
        <div className="hero__copy">
          <span className="eyebrow"><span className="dot"></span>SISTEMA DE GESTIÓN DE AMBIENTES</span>
          <h1 className="h1">
            Gestiona y reserva<br />ambientes <em>inteligentes.</em>
          </h1>
          <p className="lead">
            Consulta disponibilidad en tiempo real, administra equipamiento y optimiza el uso de
            espacios desde una sola plataforma institucional.
          </p>
          <div className="hero__cta">
            <button className="btn btn--lg btn--lila">Ver disponibilidad <Icon.Arrow /></button>
            <button className="btn btn--lg btn--ghost">Explorar ambientes</button>
          </div>
          <div className="hero__meta">
            <span><span className="ok">●</span> 47 / 64 ambientes libres ahora</span>
            <span className="sep"></span>
            <span>SSO institucional</span>
            <span className="sep"></span>
            <span>Última sync hace 12s</span>
          </div>
        </div>
        <DashboardMock />
      </div>
    </section>
  );
}

function DashboardMock() {
  // 5 días Lun–Vie, 9 filas de 30min de 09:00 a 13:30
  const days = ["Lun 12", "Mar 13", "Mié 14", "Jue 15", "Vie 16"];
  const times = ["09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00"];
  const events = [
    { day: 0, start: 1, end: 3, title: "Comité", color: "lila" },
    { day: 0, start: 5, end: 8, title: "Reunión I+D", color: "violet" },
    { day: 1, start: 0, end: 4, title: "Conf. IA", color: "cyan", note: "Auditorio" },
    { day: 1, start: 5, end: 7, title: "Defensa", color: "amber" },
    { day: 2, start: 2, end: 5, title: "Lab. Redes", color: "lila", note: "L-312" },
    { day: 2, start: 6, end: 8, title: "Mantenim.", color: "ghost" },
    { day: 3, start: 0, end: 3, title: "Reunión Decanato", color: "violet" },
    { day: 3, start: 4, end: 6, title: "Coloquio", color: "cyan" },
    { day: 4, start: 1, end: 4, title: "Inducción", color: "lila" },
    { day: 4, start: 6, end: 8, title: "Comité", color: "amber" },
  ];
  const todayCol = 2; // Mié

  return (
    <div className="dash-stage" style={{ position: "relative" }}>
      <div className="dash">
        <div className="dash__chrome">
          <div className="dash__dots"><i></i><i></i><i></i></div>
          <div className="dash__title"><b>edificiodigital.io</b> / agenda · semana 19</div>
          <div className="mono" style={{ fontSize: 11, color: "var(--ink-3)" }}>SEM 19</div>
        </div>
        <div className="dash__body">
          <div className="dash-card dash-week">
            <div className="dash-card__h">
              <b>Agenda semanal</b>
              <span>5 ambientes · 38 reservas</span>
            </div>
            <div className="dash-week__grid">
              <div></div>
              {days.map((d, i) => (
                <div key={d} className={"dash-week__hd" + (i === todayCol ? " is-today" : "")}>{d}</div>
              ))}
              {times.map((t, ri) => (
                <React.Fragment key={t}>
                  <div className="dash-week__time">{t}</div>
                  {days.map((_, ci) => (
                    <div key={ci} className="dash-week__cell"></div>
                  ))}
                </React.Fragment>
              ))}
            </div>
            {/* events overlay using absolute positioning */}
            <div style={{ position: "relative", marginTop: -((times.length) * 26 + 18), pointerEvents: "none" }}>
              <div style={{ display: "grid", gridTemplateColumns: "36px repeat(5, 1fr)", height: times.length * 26, marginTop: 18 }}>
                <div></div>
                {days.map((_, ci) => (
                  <div key={ci} className="dash-week__col" style={{ position: "relative", pointerEvents: "auto" }}>
                    {events.filter(e => e.day === ci).map((e, ei) => (
                      <div
                        key={ei}
                        className={`dash-evt evt--${e.color}`}
                        style={{
                          top: e.start * 26 + 1,
                          height: (e.end - e.start) * 26 - 2,
                        }}
                      >
                        {e.title}
                        {e.note && <small>{e.note}</small>}
                      </div>
                    ))}
                    {ci === todayCol && (
                      <div className="dash-now" style={{ top: 4 * 26 + 14 }}></div>
                    )}
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Floating widgets */}
      <div className="dash-floater dash-floater--tl">
        <span className="pulse-dot"></span>
        <div>
          <div style={{ fontWeight: 500, fontSize: 12 }}>En vivo</div>
          <div className="mono" style={{ fontSize: 10.5, color: "var(--ink-3)" }}>47 libres · 17 ocupadas</div>
        </div>
      </div>

      <div className="dash-floater dash-floater--ml">
        <div className="mono" style={{ fontSize: 10, color: "var(--ink-4)", textTransform: "uppercase", letterSpacing: "0.06em" }}>Ocupación</div>
        <div style={{ fontSize: 22, fontWeight: 500, letterSpacing: "-0.02em", marginTop: 4 }}>72<small style={{ fontSize: 12, color: "var(--ink-3)" }}>%</small></div>
        <div className="spark" style={{ marginTop: 6 }}>
          {[40,55,32,78,62,72,80,68,55,82,90,72].map((h,i) => (
            <i key={i} style={{ height: `${h}%` }}></i>
          ))}
        </div>
      </div>

      <div className="dash-floater dash-floater--br">
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "baseline", marginBottom: 8 }}>
          <b style={{ fontSize: 12, fontWeight: 500 }}>Próxima reserva</b>
          <span className="mono" style={{ fontSize: 10, color: "var(--ink-4)" }}>14:00</span>
        </div>
        <div style={{ fontSize: 13, fontWeight: 500, letterSpacing: "-0.005em" }}>Sala A-204</div>
        <div className="mono" style={{ fontSize: 11, color: "var(--ink-3)", marginTop: 3 }}>Comité Académico · 12 pers.</div>
        <div style={{ display: "flex", gap: 4, marginTop: 10 }}>
          <span className="chip" style={{ fontSize: 10, padding: "3px 7px" }}>Pantalla 65"</span>
          <span className="chip" style={{ fontSize: 10, padding: "3px 7px" }}>Videoll.</span>
        </div>
      </div>
    </div>
  );
}

// ──────────────────────────────────────────────────────────────────
// Trust strip
// ──────────────────────────────────────────────────────────────────

function Trust() {
  return (
    <div className="trust shell">
      <span>Implementado en</span>
      <span className="institution">Universidad Pacífica</span>
      <span className="institution">Centro Cívico Norte</span>
      <span className="institution">ITS Tecnológico</span>
      <span className="institution">Edificio Mariscal</span>
      <span className="institution">Cámara de Comercio</span>
    </div>
  );
}

// ──────────────────────────────────────────────────────────────────
// Features
// ──────────────────────────────────────────────────────────────────

function Features() {
  return (
    <section className="section shell" id="ambientes">
      <header className="features__head">
        <div>
          <span className="eyebrow"><span className="dot"></span>FUNCIONALIDADES</span>
          <h2 className="h2" style={{ marginTop: 16 }}>Todo lo que tu edificio necesita,<br />sin fricciones operativas.</h2>
        </div>
        <p className="lead" style={{ flex: "0 0 380px" }}>
          Una capa de software diseñada para edificios institucionales:
          rápida para usuarios, transparente para administradores.
        </p>
      </header>
      <div className="features__grid">
        {FEATURES.map((f) => (
          <article key={f.code} className="feature">
            <div className="feature__icon">{React.createElement(Icon[f.iconKey])}</div>
            <h3>{f.title}</h3>
            <p>{f.desc}</p>
            <div className="feature__tag">
              <span>{f.code} / {f.tag}</span>
              <span>Ver detalle →</span>
            </div>
          </article>
        ))}
      </div>
    </section>
  );
}

// ──────────────────────────────────────────────────────────────────
// Calendar preview (interactive)
// ──────────────────────────────────────────────────────────────────

const CAL_DAYS = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes"];
const CAL_DATES = ["12", "13", "14", "15", "16"];
const CAL_ROOM_NAMES = ["Sala A-204", "Auditorio B-001", "Lab L-312", "Coworking C-110"];
const CAL_HOURS = ["09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00"];
// 20 slots de 30min → height 28 cada slot
const SLOT_H = 28;

function CalendarPreview() {
  const [view, setView] = useState("Semana");
  const [roomIdx, setRoomIdx] = useState(0);
  const [selectedEvent, setSelectedEvent] = useState(null);

  const events = useMemo(() => CAL_EVENTS.filter(e => e.room === roomIdx), [roomIdx]);

  return (
    <section className="section shell" id="calendario">
      <header className="features__head">
        <div>
          <span className="eyebrow"><span className="dot"></span>CALENDARIO EN VIVO</span>
          <h2 className="h2" style={{ marginTop: 16 }}>Disponibilidad sincronizada,<br />sin sorpresas en la puerta.</h2>
        </div>
        <p className="lead" style={{ flex: "0 0 380px" }}>
          Vista por sala con bloques claros para libre, reservado y mantenimiento.
          Atajos de teclado para navegar y reservar sin abandonar el calendario.
        </p>
      </header>

      <div className="cal-preview">
        <div className="cal-preview__chrome">
          <div className="cal-preview__chrome-l">
            <button className="btn btn--sm btn--ghost" aria-label="Anterior"><Icon.ChevL /></button>
            <button className="btn btn--sm btn--ghost" aria-label="Siguiente"><Icon.ChevR /></button>
            <div className="mono" style={{ fontSize: 12, color: "var(--ink-2)", marginLeft: 6 }}>
              Mayo 2026 · Semana 19
            </div>
          </div>
          <div className="cal-preview__chrome-r">
            <div className="cal-tabs" role="tablist">
              {["Día", "Semana", "Mes"].map((v) => (
                <button key={v}
                  className={view === v ? "is-active" : ""}
                  onClick={() => setView(v)}>{v}</button>
              ))}
            </div>
            <div className="cal-search">
              <Icon.Search />
              <span>Buscar sala…</span>
              <kbd>⌘K</kbd>
            </div>
          </div>
        </div>

        {/* room selector */}
        <div style={{ display: "flex", gap: 6, marginBottom: 14, flexWrap: "wrap" }}>
          {CAL_ROOM_NAMES.map((r, i) => (
            <button key={r}
              onClick={() => { setRoomIdx(i); setSelectedEvent(null); }}
              className="btn btn--sm"
              style={{
                background: i === roomIdx ? "var(--lila-100)" : "var(--surface)",
                border: "1px solid " + (i === roomIdx ? "var(--lila-300)" : "var(--line)"),
                color: i === roomIdx ? "var(--lila-700)" : "var(--ink-2)",
              }}>
              {r}
            </button>
          ))}
        </div>

        <div className="cal-grid-wrap">
          <div className="cal-grid">
            <div className="cal-cell-hd"></div>
            {CAL_DAYS.map((d, i) => (
              <div key={d} className={"cal-cell-hd" + (i === 2 ? " is-today" : "")}>
                <span>{d}</span>
                <small>MAY {CAL_DATES[i]}{i === 2 ? " · HOY" : ""}</small>
              </div>
            ))}
            {CAL_HOURS.map((h, hi) => (
              <React.Fragment key={h}>
                <div className="cal-time">{h}</div>
                {CAL_DAYS.map((_, di) => (
                  <div
                    key={di}
                    className="cal-cell"
                    style={{ height: SLOT_H * 2, padding: 0, position: "relative" }}
                  >
                    {/* render events that start at this hour for this day */}
                    {events
                      .filter((e) => e.day === di && e.start >= hi * 2 && e.start < (hi + 1) * 2)
                      .map((e, ei) => {
                        const offsetTop = (e.start - hi * 2) * SLOT_H;
                        const height = (e.end - e.start) * SLOT_H - 4;
                        return (
                          <button
                            key={ei}
                            className={`cal-event evt--${e.color}`}
                            onClick={() => setSelectedEvent(e)}
                            style={{
                              top: offsetTop + 2,
                              height,
                              border: "0",
                              cursor: "default",
                              textAlign: "left",
                              width: "calc(100% - 12px)",
                              left: 6,
                            }}
                          >
                            <b>{e.title}</b>
                            <small>{e.who}</small>
                          </button>
                        );
                      })}
                  </div>
                ))}
              </React.Fragment>
            ))}
          </div>
        </div>

        <div className="cal-legend">
          <span><i style={{ background: "color-mix(in oklch, var(--lila-300) 70%, white)" }}></i>Reunión / clase</span>
          <span><i style={{ background: "color-mix(in oklch, var(--cyan-300) 70%, white)" }}></i>Evento abierto</span>
          <span><i style={{ background: "color-mix(in oklch, var(--violet-400) 50%, white)" }}></i>Comité / institucional</span>
          <span><i style={{ background: "color-mix(in oklch, var(--amber-400) 60%, white)" }}></i>Posgrado / defensa</span>
          <span><i style={{ background: "repeating-linear-gradient(45deg, var(--paper-2), var(--paper-2) 3px, var(--paper) 3px, var(--paper) 6px)", border: "1px dashed var(--line-2)" }}></i>Mantenimiento / bloqueo</span>
        </div>
      </div>

      {selectedEvent && <EventModal event={selectedEvent} room={CAL_ROOM_NAMES[roomIdx]} onClose={() => setSelectedEvent(null)} />}
    </section>
  );
}

function EventModal({ event, room, onClose }) {
  return (
    <div onClick={onClose} style={{
      position: "fixed", inset: 0, background: "color-mix(in oklch, var(--ink) 35%, transparent)",
      backdropFilter: "blur(8px)", WebkitBackdropFilter: "blur(8px)",
      zIndex: 100, display: "grid", placeItems: "center", padding: 24,
    }}>
      <div onClick={(e) => e.stopPropagation()} style={{
        background: "var(--surface)", borderRadius: "var(--r-lg)",
        border: "1px solid var(--line)",
        padding: 28, maxWidth: 440, width: "100%", boxShadow: "var(--sh-4)",
      }}>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", gap: 16 }}>
          <div>
            <div className="mono" style={{ fontSize: 11, color: "var(--ink-3)", textTransform: "uppercase", letterSpacing: "0.06em" }}>
              {room} · {CAL_DAYS[event.day]} {CAL_DATES[event.day]} MAY
            </div>
            <h3 style={{ margin: "8px 0 0", fontSize: 22, fontWeight: 500, letterSpacing: "-0.02em" }}>{event.title}</h3>
          </div>
          <button onClick={onClose} className="btn btn--sm btn--ghost" aria-label="Cerrar"><Icon.X /></button>
        </div>
        <dl style={{ display: "grid", gridTemplateColumns: "auto 1fr", gap: "10px 16px", margin: "20px 0", fontSize: 13 }}>
          <dt className="mono" style={{ color: "var(--ink-4)", textTransform: "uppercase", fontSize: 10, letterSpacing: "0.06em" }}>Horario</dt>
          <dd style={{ margin: 0 }}>{slotToTime(event.start)} – {slotToTime(event.end)}</dd>
          <dt className="mono" style={{ color: "var(--ink-4)", textTransform: "uppercase", fontSize: 10, letterSpacing: "0.06em" }}>Responsable</dt>
          <dd style={{ margin: 0 }}>{event.who}</dd>
          <dt className="mono" style={{ color: "var(--ink-4)", textTransform: "uppercase", fontSize: 10, letterSpacing: "0.06em" }}>Estado</dt>
          <dd style={{ margin: 0 }}>{event.color === "ghost" ? "Bloqueo programado" : "Confirmado"}</dd>
        </dl>
        <div style={{ display: "flex", gap: 8 }}>
          <button className="btn btn--lila">Solicitar reserva contigua</button>
          <button className="btn btn--ghost" onClick={onClose}>Cerrar</button>
        </div>
      </div>
    </div>
  );
}

function slotToTime(slot) {
  const totalMin = 9 * 60 + slot * 30;
  const h = Math.floor(totalMin / 60);
  const m = totalMin % 60;
  return `${String(h).padStart(2,"0")}:${String(m).padStart(2,"0")}`;
}

// ──────────────────────────────────────────────────────────────────
// Rooms
// ──────────────────────────────────────────────────────────────────

function Rooms() {
  return (
    <section className="section shell" id="disponibilidad">
      <header className="features__head">
        <div>
          <span className="eyebrow"><span className="dot"></span>AMBIENTES</span>
          <h2 className="h2" style={{ marginTop: 16 }}>Cada espacio,<br />con su ficha completa.</h2>
        </div>
        <p className="lead" style={{ flex: "0 0 380px" }}>
          Capacidad, equipamiento e historial de uso disponibles en una sola ficha.
          Reserva, libera o reporta incidencias en un toque.
        </p>
      </header>
      <div className="rooms__grid">
        {ROOMS.map((r) => <RoomCard key={r.ref} room={r} />)}
      </div>
    </section>
  );
}

function RoomCard({ room }) {
  const statusLabel = { free: "Libre", soon: "Pronto", busy: "Ocupado" }[room.status];
  return (
    <article className="room">
      <div className="room__media">
        <span className="room__status room__status--{room.status}" style={{}}></span>
        <span className={`room__status room__status--${room.status}`}>
          <i></i>{statusLabel}
        </span>
        <span className="room__media-label">{room.ref}.jpg</span>
      </div>
      <div className="room__body">
        <div className="room__head">
          <h3>{room.name}</h3>
          <span className="ref">{room.ref}</span>
        </div>
        <div className="room__type">{room.type}</div>
        <dl className="room__meta">
          <dt>Capacidad</dt>
          <dd style={{ gridColumn: "2 / -1" }}>{room.capacity} personas</dd>
          <dt>Ubicación</dt>
          <dd style={{ gridColumn: "2 / -1" }}>{room.floor}</dd>
        </dl>
        <div className="room__chips">
          {room.chips.map((c) => <span key={c} className="chip">{c}</span>)}
        </div>
        <div className="room__foot">
          <div className="room__next">{room.next}</div>
          <button className="btn btn--sm" style={{ color: "var(--primary)", padding: "0 4px" }}>
            Reservar →
          </button>
        </div>
      </div>
    </article>
  );
}

// ──────────────────────────────────────────────────────────────────
// Stats
// ──────────────────────────────────────────────────────────────────

function Stats() {
  const stats = [
    { num: "47", suffix: "/64", label: "Ambientes disponibles", sub: "Ahora · 5 edificios" },
    { num: "1,284", label: "Reservas activas", sub: "Últimos 30 días · +18 %" },
    { num: "12,840", label: "Usuarios registrados", sub: "Estudiantes · docentes · admin." },
    { num: "72", suffix: "%", label: "Tasa de ocupación", sub: "Promedio semanal" },
  ];
  return (
    <section className="shell" style={{ padding: "32px 0 var(--section-y)" }}>
      <div className="stats">
        <div className="stats__grid">
          {stats.map((s) => (
            <div key={s.label}>
              <div className="stat__num tabular">{s.num}{s.suffix && <small>{s.suffix}</small>}</div>
              <div className="stat__lbl">{s.label}</div>
              <div className="stat__sub">{s.sub}</div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

// ──────────────────────────────────────────────────────────────────
// Benefits / Testimonial
// ──────────────────────────────────────────────────────────────────

function Benefits() {
  return (
    <section className="section shell">
      <header className="features__head" style={{ marginBottom: 32 }}>
        <div>
          <span className="eyebrow"><span className="dot"></span>EXPERIENCIA INSTITUCIONAL</span>
          <h2 className="h2" style={{ marginTop: 16 }}>Diseñado para edificios<br />que no pueden parar.</h2>
        </div>
      </header>
      <div className="bens">
        <div className="bens__quote">
          <blockquote>
            Pasamos de coordinar salas por correo a tener un panel donde todo el campus
            sabe qué está libre y qué no — sin reuniones perdidas.
          </blockquote>
          <div className="who">
            <span className="ph">MS</span>
            <div>
              <b>Mariana Salgado</b>
              <small>Directora de Operaciones · Universidad Pacífica</small>
            </div>
          </div>
        </div>
        <div className="bens__list">
          {BENEFITS.map((b) => (
            <article key={b.num} className="benefit">
              <span className="benefit__num mono">{b.num}</span>
              <div className="benefit__txt">
                <b>{b.title}</b>
                <span>{b.desc}</span>
              </div>
            </article>
          ))}
        </div>
      </div>
    </section>
  );
}

// ──────────────────────────────────────────────────────────────────
// CTA banner + Footer
// ──────────────────────────────────────────────────────────────────

function CTABanner() {
  return (
    <section className="section shell" style={{ paddingTop: 0 }}>
      <div className="cta-banner">
        <div>
          <h2>Tu primer ambiente reservado en menos de 60 segundos.</h2>
          <p>Activa Edificio Digital en tu institución — sin migración compleja.</p>
        </div>
        <div style={{ display: "flex", gap: 10, flexWrap: "wrap" }}>
          <button className="btn btn--lg btn--lila">Solicitar demostración <Icon.Arrow /></button>
          <button className="btn btn--lg btn--ghost">Hablar con ventas</button>
        </div>
      </div>
    </section>
  );
}

function Footer() {
  return (
    <footer className="footer shell" id="contacto">
      <div className="footer__grid">
        <div>
          <a className="nav__brand" href="#" style={{ marginBottom: 8 }}>
            <span className="nav__mark"></span>
            <b>Edificio Digital</b>
          </a>
          <p>Plataforma institucional de reserva y gestión de ambientes. Diseñada para universidades, edificios corporativos y centros cívicos.</p>
        </div>
        <div className="footer__col">
          <h5>Plataforma</h5>
          <ul>
            <li><a href="#">Reservar ambiente</a></li>
            <li><a href="#">Calendario</a></li>
            <li><a href="#">Disponibilidad</a></li>
            <li><a href="#">Estadísticas</a></li>
          </ul>
        </div>
        <div className="footer__col">
          <h5>Soluciones</h5>
          <ul>
            <li><a href="#">Universidades</a></li>
            <li><a href="#">Edificios públicos</a></li>
            <li><a href="#">Coworkings</a></li>
            <li><a href="#">Auditorios</a></li>
          </ul>
        </div>
        <div className="footer__col">
          <h5>Recursos</h5>
          <ul>
            <li><a href="#">Documentación</a></li>
            <li><a href="#">Changelog</a></li>
            <li><a href="#">Estado del sistema</a></li>
            <li><a href="#">Soporte</a></li>
          </ul>
        </div>
        <div className="footer__col">
          <h5>Contacto</h5>
          <ul>
            <li><a href="mailto:hola@edificiodigital.io">hola@edificiodigital.io</a></li>
            <li><a href="#">+51 (1) 234 5678</a></li>
            <li><a href="#">Av. Institucional 1402</a></li>
            <li><a href="#">Lima, Perú</a></li>
          </ul>
        </div>
      </div>
      <div className="footer__bot">
        <span>© 2026 Edificio Digital · Todos los derechos reservados</span>
        <div className="footer__socials">
          <a href="#" aria-label="LinkedIn">
            <svg width="14" height="14" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.5"><rect x="2" y="2" width="12" height="12" rx="2" /><path d="M5 7v5M5 5v.01M8 12V7M11 12v-3a2 2 0 0 0-3-2v0" strokeLinecap="round" /></svg>
          </a>
          <a href="#" aria-label="X">
            <svg width="14" height="14" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round"><path d="M3 3l10 10M13 3L3 13" /></svg>
          </a>
          <a href="#" aria-label="GitHub">
            <svg width="14" height="14" viewBox="0 0 16 16" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinejoin="round"><path d="M8 1.5C4.4 1.5 1.5 4.4 1.5 8c0 2.9 1.9 5.3 4.4 6.2.3.05.4-.15.4-.3v-1c-1.8.4-2.2-.85-2.2-.85-.3-.75-.7-.95-.7-.95-.6-.4.05-.4.05-.4.65.05 1 .65 1 .65.6 1 1.55.7 1.9.55.05-.4.2-.7.4-.85-1.4-.15-2.9-.7-2.9-3.15 0-.7.25-1.25.65-1.7-.05-.15-.3-.8.05-1.65 0 0 .55-.15 1.7.65a6 6 0 0 1 3 0c1.15-.8 1.7-.65 1.7-.65.35.85.1 1.5.05 1.65.4.45.65 1 .65 1.7 0 2.45-1.5 3-2.9 3.15.2.2.4.55.4 1.15v1.7c0 .15.1.35.4.3 2.5-.9 4.4-3.3 4.4-6.2 0-3.6-2.9-6.5-6.5-6.5z" /></svg>
          </a>
        </div>
      </div>
    </footer>
  );
}

// ──────────────────────────────────────────────────────────────────
// Tweaks
// ──────────────────────────────────────────────────────────────────

const TWEAK_DEFAULTS = /*EDITMODE-BEGIN*/{
  "palette": "lila",
  "density": "regular",
  "dark": false,
  "accent": "cyan",
  "heroVariant": "dashboard"
}/*EDITMODE-END*/;

const PALETTES = {
  lila:    { 400: "oklch(0.72 0.14 295)", 500: "oklch(0.58 0.16 295)", 600: "oklch(0.48 0.16 295)", 700: "oklch(0.38 0.13 295)", 100: "oklch(0.965 0.018 295)", 200: "oklch(0.92 0.04 295)", 300: "oklch(0.84 0.08 295)" },
  indigo:  { 400: "oklch(0.68 0.18 270)", 500: "oklch(0.55 0.20 270)", 600: "oklch(0.45 0.20 270)", 700: "oklch(0.36 0.16 270)", 100: "oklch(0.965 0.020 270)", 200: "oklch(0.92 0.05 270)", 300: "oklch(0.84 0.10 270)" },
  emerald: { 400: "oklch(0.72 0.14 165)", 500: "oklch(0.58 0.16 165)", 600: "oklch(0.48 0.16 165)", 700: "oklch(0.38 0.13 165)", 100: "oklch(0.965 0.018 165)", 200: "oklch(0.92 0.04 165)", 300: "oklch(0.84 0.08 165)" },
  slate:   { 400: "oklch(0.68 0.04 270)", 500: "oklch(0.50 0.04 270)", 600: "oklch(0.40 0.04 270)", 700: "oklch(0.30 0.04 270)", 100: "oklch(0.965 0.005 270)", 200: "oklch(0.92 0.01 270)", 300: "oklch(0.84 0.02 270)" },
};

const ACCENTS = {
  cyan:   { 300: "oklch(0.86 0.10 220)", 400: "oklch(0.78 0.13 220)", 500: "oklch(0.68 0.15 220)" },
  violet: { 300: "oklch(0.80 0.15 305)", 400: "oklch(0.66 0.18 305)", 500: "oklch(0.55 0.20 305)" },
  amber:  { 300: "oklch(0.88 0.10 80)",  400: "oklch(0.82 0.13 80)",  500: "oklch(0.72 0.15 80)"  },
  rose:   { 300: "oklch(0.82 0.13 18)",  400: "oklch(0.72 0.16 18)",  500: "oklch(0.62 0.18 18)"  },
};

function applyTweaks(t) {
  const root = document.documentElement;
  const p = PALETTES[t.palette] || PALETTES.lila;
  Object.entries(p).forEach(([k, v]) => root.style.setProperty(`--lila-${k}`, v));
  const a = ACCENTS[t.accent] || ACCENTS.cyan;
  Object.entries(a).forEach(([k, v]) => root.style.setProperty(`--cyan-${k}`, v));
  const dMap = { compact: 0.75, regular: 1, comfy: 1.2 };
  root.style.setProperty("--d", dMap[t.density] || 1);
  root.dataset.theme = t.dark ? "dark" : "light";
}

function App() {
  const [t, setTweak] = useTweaks(TWEAK_DEFAULTS);

  useEffect(() => { applyTweaks(t); }, [t]);

  return (
    <>
      <Navbar />
      <Hero />
      <Trust />
      <Features />
      <CalendarPreview />
      <Rooms />
      <Stats />
      <Benefits />
      <CTABanner />
      <Footer />

      <TweaksPanel title="Tweaks">
        <TweakSection label="Tema" />
        <TweakRadio label="Modo" value={t.dark ? "Oscuro" : "Claro"}
          options={["Claro", "Oscuro"]}
          onChange={(v) => setTweak("dark", v === "Oscuro")} />
        <TweakColor label="Paleta institucional"
          value={paletteSwatch(t.palette)}
          options={[
            paletteSwatch("lila"),
            paletteSwatch("indigo"),
            paletteSwatch("emerald"),
            paletteSwatch("slate"),
          ]}
          onChange={(v) => {
            const found = Object.keys(PALETTES).find(k => paletteSwatch(k)[0] === v[0]);
            if (found) setTweak("palette", found);
          }} />
        <TweakColor label="Acento"
          value={accentSwatch(t.accent)}
          options={[
            accentSwatch("cyan"),
            accentSwatch("violet"),
            accentSwatch("amber"),
            accentSwatch("rose"),
          ]}
          onChange={(v) => {
            const found = Object.keys(ACCENTS).find(k => accentSwatch(k)[0] === v[0]);
            if (found) setTweak("accent", found);
          }} />

        <TweakSection label="Densidad" />
        <TweakRadio label="Espaciado" value={t.density}
          options={["compact", "regular", "comfy"]}
          onChange={(v) => setTweak("density", v)} />
      </TweaksPanel>
    </>
  );
}

// turn palette key into a 3-color swatch (oklch → hex via canvas trick? no — pass oklch)
function paletteSwatch(key) {
  const p = PALETTES[key];
  return [
    `oklch(0.58 ${key === "slate" ? 0.04 : 0.16} ${key === "lila" ? 295 : key === "indigo" ? 270 : key === "emerald" ? 165 : 270})`,
    `oklch(0.84 ${key === "slate" ? 0.02 : 0.08} ${key === "lila" ? 295 : key === "indigo" ? 270 : key === "emerald" ? 165 : 270})`,
    `oklch(0.38 ${key === "slate" ? 0.04 : 0.13} ${key === "lila" ? 295 : key === "indigo" ? 270 : key === "emerald" ? 165 : 270})`,
  ];
}
function accentSwatch(key) {
  const hueMap = { cyan: 220, violet: 305, amber: 80, rose: 18 };
  const h = hueMap[key];
  return [
    `oklch(0.78 0.13 ${h})`,
    `oklch(0.86 0.10 ${h})`,
  ];
}

// mount
ReactDOM.createRoot(document.getElementById("root")).render(<App />);
