using edificio_digital.Entity.Model;
using edificio_digital.Entity.Model.Usuario;
using Microsoft.EntityFrameworkCore;

namespace edificio_digital.Entity.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Sede> Sedes => Set<Sede>();
    public DbSet<Edificio> Edificios => Set<Edificio>();
    public DbSet<Piso> Pisos => Set<Piso>();
    public DbSet<Dependencia> Dependencias => Set<Dependencia>();
    public DbSet<Ambiente> Ambientes => Set<Ambiente>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<HistorialTipoAmbiente> HistorialTiposAmbiente => Set<HistorialTipoAmbiente>();
    public DbSet<BloqueoAmbiente> BloqueosAmbiente => Set<BloqueoAmbiente>();
    public DbSet<HistorialUsoEquipo> HistorialUsoEquipos => Set<HistorialUsoEquipo>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<UsuarioRol> UsuariosRoles => Set<UsuarioRol>();
    public DbSet<RolPermiso> RolesPermisos => Set<RolPermiso>();
    public DbSet<FranjaHoraria> FranjasHorarias => Set<FranjaHoraria>();
    public DbSet<DisponibilidadAmbiente> DisponibilidadesAmbiente => Set<DisponibilidadAmbiente>();
    public DbSet<PrioridadReserva> PrioridadesReserva => Set<PrioridadReserva>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<ReservaCalendarioDia> ReservasCalendarioDia => Set<ReservaCalendarioDia>();
    public DbSet<HistorialUsoAmbiente> HistorialUsoAmbientes => Set<HistorialUsoAmbiente>();
    public DbSet<BitacoraAuditoria> BitacoraAuditoria => Set<BitacoraAuditoria>();
    public DbSet<HistorialLogin> HistorialLogin => Set<HistorialLogin>();
    public DbSet<HistorialConfiguracion> HistorialConfiguracion => Set<HistorialConfiguracion>();
    public DbSet<HistorialMovimientoGlobal> HistorialMovimientoGlobal => Set<HistorialMovimientoGlobal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sede>(entity =>
        {
            entity.ToTable("sedes", "estructura");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Codigo).HasColumnName("codigo").IsRequired();
            entity.Property(x => x.Nombre).HasColumnName("nombre").IsRequired();
            entity.Property(x => x.Direccion).HasColumnName("direccion");
            entity.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
            entity.HasIndex(x => x.Codigo).IsUnique();
        });

        modelBuilder.Entity<Edificio>(entity =>
        {
            entity.ToTable("edificios", "estructura");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.SedeId).HasColumnName("sede_id");
            entity.Property(x => x.Codigo).HasColumnName("codigo").IsRequired();
            entity.Property(x => x.Nombre).HasColumnName("nombre").IsRequired();
            entity.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
            entity.HasIndex(x => new { x.SedeId, x.Codigo }).IsUnique();
            entity.HasOne(x => x.Sede).WithMany(x => x.Edificios).HasForeignKey(x => x.SedeId);
        });

        modelBuilder.Entity<Piso>(entity =>
        {
            entity.ToTable("pisos", "estructura");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.EdificioId).HasColumnName("edificio_id");
            entity.Property(x => x.Numero).HasColumnName("numero").IsRequired();
            entity.Property(x => x.Nombre).HasColumnName("nombre");
            entity.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
            entity.HasIndex(x => new { x.EdificioId, x.Numero }).IsUnique();
            entity.HasOne(x => x.Edificio).WithMany(x => x.Pisos).HasForeignKey(x => x.EdificioId);
        });

        modelBuilder.Entity<Dependencia>(entity =>
        {
            entity.ToTable("dependencias", "estructura");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.SedeId).HasColumnName("sede_id");
            entity.Property(x => x.Codigo).HasColumnName("codigo").IsRequired();
            entity.Property(x => x.Nombre).HasColumnName("nombre").IsRequired();
            entity.Property(x => x.Tipo).HasColumnName("tipo").IsRequired();
            entity.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
            entity.HasIndex(x => new { x.SedeId, x.Codigo }).IsUnique();
            entity.HasOne(x => x.Sede).WithMany(x => x.Dependencias).HasForeignKey(x => x.SedeId);
        });

        modelBuilder.Entity<Ambiente>(entity =>
        {
            entity.ToTable("ambientes", "estructura");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.PisoId).HasColumnName("piso_id");
            entity.Property(x => x.DependenciaId).HasColumnName("dependencia_id");
            entity.Property(x => x.Codigo).HasColumnName("codigo").IsRequired();
            entity.Property(x => x.Nombre).HasColumnName("nombre").IsRequired();
            entity.Property(x => x.Tipo).HasColumnName("tipo").IsRequired();
            entity.Property(x => x.AforoMaximo).HasColumnName("aforo_maximo").IsRequired();
            entity.Property(x => x.Estado).HasColumnName("estado").IsRequired();
            entity.Property(x => x.Publicable).HasColumnName("publicable").HasDefaultValue(false).IsRequired();
            entity.Property(x => x.Observacion).HasColumnName("observacion");
            entity.HasIndex(x => new { x.PisoId, x.Codigo }).IsUnique();
            entity.HasOne(x => x.Piso).WithMany(x => x.Ambientes).HasForeignKey(x => x.PisoId);
            entity.HasOne(x => x.Dependencia).WithMany(x => x.Ambientes).HasForeignKey(x => x.DependenciaId);
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.ToTable("equipos", "recursos");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AmbienteId).HasColumnName("ambiente_id");
            entity.Property(x => x.DependenciaId).HasColumnName("dependencia_id");
            entity.Property(x => x.CodigoPatrimonial).HasColumnName("codigo_patrimonial").IsRequired();
            entity.Property(x => x.Serie).HasColumnName("serie");
            entity.Property(x => x.Nombre).HasColumnName("nombre").IsRequired();
            entity.Property(x => x.Tipo).HasColumnName("tipo").IsRequired();
            entity.Property(x => x.Estado).HasColumnName("estado").IsRequired();
            entity.HasIndex(x => x.CodigoPatrimonial).IsUnique();
            entity.HasOne(x => x.Ambiente).WithMany(x => x.Equipos).HasForeignKey(x => x.AmbienteId);
            entity.HasOne(x => x.Dependencia).WithMany(x => x.Equipos).HasForeignKey(x => x.DependenciaId);
        });

        modelBuilder.Entity<HistorialTipoAmbiente>(entity =>
        {
            entity.ToTable("historial_tipo_ambiente", "recursos");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AmbienteId).HasColumnName("ambiente_id");
            entity.Property(x => x.TipoAnterior).HasColumnName("tipo_anterior").IsRequired();
            entity.Property(x => x.TipoNuevo).HasColumnName("tipo_nuevo").IsRequired();
            entity.Property(x => x.MotivoCambio).HasColumnName("motivo_cambio").IsRequired();
            entity.Property(x => x.FechaCambio).HasColumnName("fecha_cambio").IsRequired();
            entity.HasOne(x => x.Ambiente).WithMany(x => x.HistorialTipos).HasForeignKey(x => x.AmbienteId);
        });

        modelBuilder.Entity<BloqueoAmbiente>(entity =>
        {
            entity.ToTable("bloqueos_ambiente", "recursos");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AmbienteId).HasColumnName("ambiente_id");
            entity.Property(x => x.BloqueadoPorUsuarioId).HasColumnName("bloqueado_por_usuario_id");
            entity.Property(x => x.MotivoBloqueo).HasColumnName("motivo_bloqueo").IsRequired();
            entity.Property(x => x.BloqueadoEn).HasColumnName("bloqueado_en").IsRequired();
            entity.Property(x => x.DesbloqueadoPorUsuarioId).HasColumnName("desbloqueado_por_usuario_id");
            entity.Property(x => x.MotivoDesbloqueo).HasColumnName("motivo_desbloqueo");
            entity.Property(x => x.DesbloqueadoEn).HasColumnName("desbloqueado_en");
            entity.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
            entity.HasOne(x => x.Ambiente).WithMany(x => x.Bloqueos).HasForeignKey(x => x.AmbienteId);
            entity.HasOne(x => x.BloqueadoPorUsuario).WithMany().HasForeignKey(x => x.BloqueadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.DesbloqueadoPorUsuario).WithMany().HasForeignKey(x => x.DesbloqueadoPorUsuarioId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<HistorialUsoEquipo>(entity =>
        {
            entity.ToTable("historial_uso_equipo", "recursos");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.EquipoId).HasColumnName("equipo_id");
            entity.Property(x => x.AmbienteId).HasColumnName("ambiente_id");
            entity.Property(x => x.UsuarioId).HasColumnName("usuario_id");
            entity.Property(x => x.Inicio).HasColumnName("inicio").IsRequired();
            entity.Property(x => x.Fin).HasColumnName("fin");
            entity.Property(x => x.Motivo).HasColumnName("motivo");
            entity.HasOne(x => x.Equipo).WithMany(x => x.HistorialUso).HasForeignKey(x => x.EquipoId);
            entity.HasOne(x => x.Ambiente).WithMany(x => x.HistorialUsoEquipos).HasForeignKey(x => x.AmbienteId);
            entity.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios", "usuario");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");

            entity.Property(x => x.NombreUsuario).HasColumnName("nombre_usuario").HasMaxLength(50).IsRequired();
            entity.Property(x => x.Email).HasColumnName("correo").HasMaxLength(200).IsRequired();
            entity.Property(x => x.NombreCompleto).HasColumnName("nombre_completo").HasMaxLength(250).IsRequired();
            entity.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(50).IsRequired();
            entity.Property(x => x.Activo).HasColumnName("activo").IsRequired();
            entity.Property(x => x.DependenciaId).HasColumnName("dependencia_id");
            entity.Property(x => x.Contrasena).HasColumnName("contrasena").HasMaxLength(250).IsRequired();

            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.NombreUsuario).IsUnique();
            entity.HasOne<Dependencia>().WithMany(x => x.Usuarios).HasForeignKey(x => x.DependenciaId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("roles", "usuario");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Codigo).HasColumnName("codigo").HasMaxLength(100).IsRequired();
            entity.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();

            entity.HasIndex(x => x.Codigo).IsUnique();
        });

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.ToTable("usuarios_roles", "usuario");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.UsuarioId).HasColumnName("usuario_id");
            entity.Property(x => x.RolId).HasColumnName("rol_id");
            entity.Property(x => x.VigenciaDesde).HasColumnName("vigencia_desde");
            entity.Property(x => x.VigenciaHasta).HasColumnName("vigencia_hasta");
            entity.HasIndex(x => new { x.UsuarioId, x.RolId, x.VigenciaDesde }).IsUnique();

            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.UsuarioRoles)
                .HasForeignKey(x => x.UsuarioId);

            entity.HasOne(x => x.Rol)
                .WithMany(x => x.UsuarioRoles)
                .HasForeignKey(x => x.RolId);
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.ToTable("permisos", "seguridad");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Codigo).HasColumnName("codigo").IsRequired();
            entity.Property(x => x.Recurso).HasColumnName("recurso").IsRequired();
            entity.Property(x => x.Accion).HasColumnName("accion").IsRequired();
            entity.HasIndex(x => x.Codigo).IsUnique();
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.ToTable("roles_permisos", "seguridad");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.RolId).HasColumnName("rol_id");
            entity.Property(x => x.PermisoId).HasColumnName("permiso_id");
            entity.HasIndex(x => new { x.RolId, x.PermisoId }).IsUnique();
            entity.HasOne(x => x.Rol).WithMany().HasForeignKey(x => x.RolId);
            entity.HasOne(x => x.Permiso).WithMany(x => x.RolesPermisos).HasForeignKey(x => x.PermisoId);
        });

        modelBuilder.Entity<FranjaHoraria>(entity =>
        {
            entity.ToTable("franjas_horarias", "seguridad");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.DiaSemana).HasColumnName("dia_semana").IsRequired();
            entity.Property(x => x.HoraInicio).HasColumnName("hora_inicio").IsRequired();
            entity.Property(x => x.HoraFin).HasColumnName("hora_fin").IsRequired();
            entity.HasIndex(x => new { x.DiaSemana, x.HoraInicio, x.HoraFin }).IsUnique();
        });

        modelBuilder.Entity<DisponibilidadAmbiente>(entity =>
        {
            entity.ToTable("disponibilidades_ambiente", "seguridad");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AmbienteId).HasColumnName("ambiente_id");
            entity.Property(x => x.FranjaHorariaId).HasColumnName("franja_horaria_id");
            entity.Property(x => x.VigenciaDesde).HasColumnName("vigencia_desde").IsRequired();
            entity.Property(x => x.VigenciaHasta).HasColumnName("vigencia_hasta");
            entity.Property(x => x.Disponible).HasColumnName("disponible").HasDefaultValue(true).IsRequired();
            entity.HasOne(x => x.Ambiente).WithMany(x => x.Disponibilidades).HasForeignKey(x => x.AmbienteId);
            entity.HasOne(x => x.FranjaHoraria).WithMany(x => x.Disponibilidades).HasForeignKey(x => x.FranjaHorariaId);
        });

        modelBuilder.Entity<PrioridadReserva>(entity =>
        {
            entity.ToTable("prioridades_reserva", "seguridad");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.RolId).HasColumnName("rol_id");
            entity.Property(x => x.TipoUsuario).HasColumnName("tipo_usuario");
            entity.Property(x => x.NivelPrioridad).HasColumnName("nivel_prioridad").IsRequired();
            entity.Property(x => x.AntelacionHoras).HasColumnName("antelacion_horas").HasDefaultValue(0).IsRequired();
            entity.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
            entity.HasOne(x => x.Rol).WithMany().HasForeignKey(x => x.RolId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.ToTable("reservas", "reservas");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AmbienteId).HasColumnName("ambiente_id");
            entity.Property(x => x.SolicitanteId).HasColumnName("solicitante_id");
            entity.Property(x => x.DependenciaSolicitanteId).HasColumnName("dependencia_solicitante_id");
            entity.Property(x => x.TipoReserva).HasColumnName("tipo_reserva").IsRequired();
            entity.Property(x => x.FechaInicio).HasColumnName("fecha_inicio").IsRequired();
            entity.Property(x => x.FechaFin).HasColumnName("fecha_fin").IsRequired();
            entity.Property(x => x.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(x => x.HoraFin).HasColumnName("hora_fin");
            entity.Property(x => x.TipoFranja).HasColumnName("tipo_franja").IsRequired();
            entity.Property(x => x.ReglaRecurrencia).HasColumnName("regla_recurrencia");
            entity.Property(x => x.Estado).HasColumnName("estado").IsRequired();
            entity.Property(x => x.PrioridadAplicada).HasColumnName("prioridad_aplicada").HasDefaultValue(0).IsRequired();
            entity.Property(x => x.Motivo).HasColumnName("motivo").IsRequired();
            entity.HasOne(x => x.Ambiente).WithMany(x => x.Reservas).HasForeignKey(x => x.AmbienteId);
            entity.HasOne(x => x.Solicitante).WithMany().HasForeignKey(x => x.SolicitanteId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.DependenciaSolicitante).WithMany().HasForeignKey(x => x.DependenciaSolicitanteId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ReservaCalendarioDia>(entity =>
        {
            entity.ToTable("reservas_calendario_dia", "reservas");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.ReservaId).HasColumnName("reserva_id");
            entity.Property(x => x.Fecha).HasColumnName("fecha").IsRequired();
            entity.Property(x => x.HoraInicio).HasColumnName("hora_inicio").IsRequired();
            entity.Property(x => x.HoraFin).HasColumnName("hora_fin").IsRequired();
            entity.Property(x => x.Estado).HasColumnName("estado").IsRequired();
            entity.HasIndex(x => new { x.ReservaId, x.Fecha, x.HoraInicio, x.HoraFin });
            entity.HasOne(x => x.Reserva).WithMany(x => x.CalendarioDias).HasForeignKey(x => x.ReservaId);
        });

        modelBuilder.Entity<HistorialUsoAmbiente>(entity =>
        {
            entity.ToTable("historial_uso_ambiente", "reservas");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.AmbienteId).HasColumnName("ambiente_id");
            entity.Property(x => x.UsuarioId).HasColumnName("usuario_id");
            entity.Property(x => x.ReservaId).HasColumnName("reserva_id");
            entity.Property(x => x.Inicio).HasColumnName("inicio").IsRequired();
            entity.Property(x => x.Fin).HasColumnName("fin");
            entity.Property(x => x.TipoUso).HasColumnName("tipo_uso").IsRequired();
            entity.Property(x => x.Observacion).HasColumnName("observacion");
            entity.HasOne(x => x.Ambiente).WithMany(x => x.HistorialUsoAmbientes).HasForeignKey(x => x.AmbienteId);
            entity.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(x => x.Reserva).WithMany(x => x.HistorialUsoAmbientes).HasForeignKey(x => x.ReservaId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<BitacoraAuditoria>(entity =>
        {
            entity.ToTable("bitacora_auditoria", "auditoria");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.FechaHora).HasColumnName("fecha_hora").IsRequired();
            entity.Property(x => x.Tabla).HasColumnName("tabla").IsRequired();
            entity.Property(x => x.RegistroId).HasColumnName("registro_id").IsRequired();
            entity.Property(x => x.Accion).HasColumnName("accion").IsRequired();
            entity.Property(x => x.ValoresAnterioresJson).HasColumnName("valores_anteriores_json").HasColumnType("jsonb");
            entity.Property(x => x.ValoresNuevosJson).HasColumnName("valores_nuevos_json").HasColumnType("jsonb");
            entity.Property(x => x.DireccionIp).HasColumnName("direccion_ip");
            entity.Property(x => x.AgenteUsuario).HasColumnName("agente_usuario");
            entity.Property(x => x.ActorUsuarioId).HasColumnName("actor_usuario_id");
            entity.HasOne(x => x.ActorUsuario).WithMany().HasForeignKey(x => x.ActorUsuarioId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<HistorialLogin>(entity =>
        {
            entity.ToTable("historial_login", "auditoria");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.FechaHora).HasColumnName("fecha_hora").IsRequired();
            entity.Property(x => x.UsuarioIntentado).HasColumnName("usuario_intentado").IsRequired();
            entity.Property(x => x.Exitoso).HasColumnName("exitoso").IsRequired();
            entity.Property(x => x.Causa).HasColumnName("causa");
            entity.Property(x => x.DetalleError).HasColumnName("detalle_error");
            entity.Property(x => x.DireccionIp).HasColumnName("direccion_ip");
            entity.Property(x => x.AgenteUsuario).HasColumnName("agente_usuario");
        });

        modelBuilder.Entity<HistorialConfiguracion>(entity =>
        {
            entity.ToTable("historial_configuracion", "auditoria");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.FechaHora).HasColumnName("fecha_hora").IsRequired();
            entity.Property(x => x.Modulo).HasColumnName("modulo").IsRequired();
            entity.Property(x => x.ClaveConfiguracion).HasColumnName("clave_configuracion").IsRequired();
            entity.Property(x => x.ValorAnterior).HasColumnName("valor_anterior");
            entity.Property(x => x.ValorNuevo).HasColumnName("valor_nuevo");
            entity.Property(x => x.Accion).HasColumnName("accion").IsRequired();
            entity.Property(x => x.EjecutadoPor).HasColumnName("ejecutado_por");
            entity.Property(x => x.Observacion).HasColumnName("observacion");
        });

        modelBuilder.Entity<HistorialMovimientoGlobal>(entity =>
        {
            entity.ToTable("historial_movimiento_global", "auditoria");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.FechaHora).HasColumnName("fecha_hora").IsRequired();
            entity.Property(x => x.Entidad).HasColumnName("entidad").IsRequired();
            entity.Property(x => x.EntidadId).HasColumnName("entidad_id");
            entity.Property(x => x.Accion).HasColumnName("accion").IsRequired();
            entity.Property(x => x.CreadoPor).HasColumnName("creado_por");
            entity.Property(x => x.ModificadoPor).HasColumnName("modificado_por");
            entity.Property(x => x.EliminadoPor).HasColumnName("eliminado_por");
            entity.Property(x => x.Motivo).HasColumnName("motivo");
            entity.Property(x => x.DetalleJson).HasColumnName("detalle_json").HasColumnType("jsonb");
        });
    }
}

