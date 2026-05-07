using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edificio_digital.Entity.Migrations
{
    /// <inheritdoc />
    public partial class InicialClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "estructura");

            migrationBuilder.EnsureSchema(
                name: "auditoria");

            migrationBuilder.EnsureSchema(
                name: "recursos");

            migrationBuilder.EnsureSchema(
                name: "seguridad");

            migrationBuilder.EnsureSchema(
                name: "reservas");

            migrationBuilder.EnsureSchema(
                name: "usuario");

            migrationBuilder.CreateTable(
                name: "franjas_horarias",
                schema: "seguridad",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    dia_semana = table.Column<int>(type: "integer", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    hora_fin = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_franjas_horarias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "historial_configuracion",
                schema: "auditoria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modulo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    clave_configuracion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    valor_anterior = table.Column<string>(type: "text", nullable: true),
                    valor_nuevo = table.Column<string>(type: "text", nullable: true),
                    accion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ejecutado_por = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_configuracion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "historial_login",
                schema: "auditoria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    usuario_intentado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    exitoso = table.Column<bool>(type: "boolean", nullable: false),
                    causa = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    detalle_error = table.Column<string>(type: "text", nullable: true),
                    direccion_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    agente_usuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_login", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "historial_movimiento_global",
                schema: "auditoria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    entidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    entidad_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    accion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    creado_por = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    modificado_por = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    eliminado_por = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    detalle_json = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_movimiento_global", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permisos",
                schema: "seguridad",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    recurso = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    accion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permisos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sedes",
                schema: "estructura",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sedes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prioridades_reserva",
                schema: "seguridad",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tipo_usuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nivel_prioridad = table.Column<int>(type: "integer", nullable: false),
                    antelacion_horas = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prioridades_reserva", x => x.id);
                    table.ForeignKey(
                        name: "FK_prioridades_reserva_roles_rol_id",
                        column: x => x.rol_id,
                        principalSchema: "usuario",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "roles_permisos",
                schema: "seguridad",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false),
                    permiso_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_permisos", x => x.id);
                    table.ForeignKey(
                        name: "FK_roles_permisos_permisos_permiso_id",
                        column: x => x.permiso_id,
                        principalSchema: "seguridad",
                        principalTable: "permisos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roles_permisos_roles_rol_id",
                        column: x => x.rol_id,
                        principalSchema: "usuario",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dependencias",
                schema: "estructura",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sede_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dependencias", x => x.id);
                    table.ForeignKey(
                        name: "FK_dependencias_sedes_sede_id",
                        column: x => x.sede_id,
                        principalSchema: "estructura",
                        principalTable: "sedes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "edificios",
                schema: "estructura",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sede_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_edificios", x => x.id);
                    table.ForeignKey(
                        name: "FK_edificios_sedes_sede_id",
                        column: x => x.sede_id,
                        principalSchema: "estructura",
                        principalTable: "sedes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre_usuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    nombre_completo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    dependencia_id = table.Column<Guid>(type: "uuid", nullable: true),
                    contrasena = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuarios_dependencias_dependencia_id",
                        column: x => x.dependencia_id,
                        principalSchema: "estructura",
                        principalTable: "dependencias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "pisos",
                schema: "estructura",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    edificio_id = table.Column<Guid>(type: "uuid", nullable: false),
                    numero = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pisos", x => x.id);
                    table.ForeignKey(
                        name: "FK_pisos_edificios_edificio_id",
                        column: x => x.edificio_id,
                        principalSchema: "estructura",
                        principalTable: "edificios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bitacora_auditoria",
                schema: "auditoria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tabla = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    registro_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    accion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    valores_anteriores_json = table.Column<string>(type: "jsonb", nullable: true),
                    valores_nuevos_json = table.Column<string>(type: "jsonb", nullable: true),
                    direccion_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    agente_usuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    actor_usuario_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bitacora_auditoria", x => x.id);
                    table.ForeignKey(
                        name: "FK_bitacora_auditoria_usuarios_actor_usuario_id",
                        column: x => x.actor_usuario_id,
                        principalSchema: "usuario",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_roles",
                schema: "usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vigencia_desde = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vigencia_hasta = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuarios_roles_roles_rol_id",
                        column: x => x.rol_id,
                        principalSchema: "usuario",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuarios_roles_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalSchema: "usuario",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ambientes",
                schema: "estructura",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    piso_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dependencia_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    aforo_maximo = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    publicable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ambientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_ambientes_dependencias_dependencia_id",
                        column: x => x.dependencia_id,
                        principalSchema: "estructura",
                        principalTable: "dependencias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ambientes_pisos_piso_id",
                        column: x => x.piso_id,
                        principalSchema: "estructura",
                        principalTable: "pisos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bloqueos_ambiente",
                schema: "recursos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bloqueado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    motivo_bloqueo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    bloqueado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    desbloqueado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    motivo_desbloqueo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    desbloqueado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bloqueos_ambiente", x => x.id);
                    table.ForeignKey(
                        name: "FK_bloqueos_ambiente_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalSchema: "estructura",
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bloqueos_ambiente_usuarios_bloqueado_por_usuario_id",
                        column: x => x.bloqueado_por_usuario_id,
                        principalSchema: "usuario",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bloqueos_ambiente_usuarios_desbloqueado_por_usuario_id",
                        column: x => x.desbloqueado_por_usuario_id,
                        principalSchema: "usuario",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "disponibilidades_ambiente",
                schema: "seguridad",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    franja_horaria_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vigencia_desde = table.Column<DateOnly>(type: "date", nullable: false),
                    vigencia_hasta = table.Column<DateOnly>(type: "date", nullable: true),
                    disponible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disponibilidades_ambiente", x => x.id);
                    table.ForeignKey(
                        name: "FK_disponibilidades_ambiente_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalSchema: "estructura",
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_disponibilidades_ambiente_franjas_horarias_franja_horaria_id",
                        column: x => x.franja_horaria_id,
                        principalSchema: "seguridad",
                        principalTable: "franjas_horarias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "equipos",
                schema: "recursos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dependencia_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo_patrimonial = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    serie = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipos", x => x.id);
                    table.ForeignKey(
                        name: "FK_equipos_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalSchema: "estructura",
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_equipos_dependencias_dependencia_id",
                        column: x => x.dependencia_id,
                        principalSchema: "estructura",
                        principalTable: "dependencias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "historial_tipo_ambiente",
                schema: "recursos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_anterior = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tipo_nuevo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    motivo_cambio = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    fecha_cambio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_tipo_ambiente", x => x.id);
                    table.ForeignKey(
                        name: "FK_historial_tipo_ambiente_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalSchema: "estructura",
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                schema: "reservas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    solicitante_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dependencia_solicitante_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tipo_reserva = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    hora_fin = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    tipo_franja = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    regla_recurrencia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    prioridad_aplicada = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservas_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalSchema: "estructura",
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservas_dependencias_dependencia_solicitante_id",
                        column: x => x.dependencia_solicitante_id,
                        principalSchema: "estructura",
                        principalTable: "dependencias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_reservas_usuarios_solicitante_id",
                        column: x => x.solicitante_id,
                        principalSchema: "usuario",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "historial_uso_equipo",
                schema: "recursos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_uso_equipo", x => x.id);
                    table.ForeignKey(
                        name: "FK_historial_uso_equipo_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalSchema: "estructura",
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_historial_uso_equipo_equipos_equipo_id",
                        column: x => x.equipo_id,
                        principalSchema: "recursos",
                        principalTable: "equipos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_historial_uso_equipo_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalSchema: "usuario",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "historial_uso_ambiente",
                schema: "reservas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    reserva_id = table.Column<Guid>(type: "uuid", nullable: true),
                    inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tipo_uso = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_uso_ambiente", x => x.id);
                    table.ForeignKey(
                        name: "FK_historial_uso_ambiente_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalSchema: "estructura",
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_historial_uso_ambiente_reservas_reserva_id",
                        column: x => x.reserva_id,
                        principalSchema: "reservas",
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_historial_uso_ambiente_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalSchema: "usuario",
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "reservas_calendario_dia",
                schema: "reservas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reserva_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    hora_fin = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas_calendario_dia", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservas_calendario_dia_reservas_reserva_id",
                        column: x => x.reserva_id,
                        principalSchema: "reservas",
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ambientes_dependencia_id",
                schema: "estructura",
                table: "ambientes",
                column: "dependencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_ambientes_piso_id_codigo",
                schema: "estructura",
                table: "ambientes",
                columns: new[] { "piso_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bitacora_auditoria_actor_usuario_id",
                schema: "auditoria",
                table: "bitacora_auditoria",
                column: "actor_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_bloqueos_ambiente_ambiente_id",
                schema: "recursos",
                table: "bloqueos_ambiente",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_bloqueos_ambiente_bloqueado_por_usuario_id",
                schema: "recursos",
                table: "bloqueos_ambiente",
                column: "bloqueado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_bloqueos_ambiente_desbloqueado_por_usuario_id",
                schema: "recursos",
                table: "bloqueos_ambiente",
                column: "desbloqueado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_dependencias_sede_id_codigo",
                schema: "estructura",
                table: "dependencias",
                columns: new[] { "sede_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidades_ambiente_ambiente_id",
                schema: "seguridad",
                table: "disponibilidades_ambiente",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidades_ambiente_franja_horaria_id",
                schema: "seguridad",
                table: "disponibilidades_ambiente",
                column: "franja_horaria_id");

            migrationBuilder.CreateIndex(
                name: "IX_edificios_sede_id_codigo",
                schema: "estructura",
                table: "edificios",
                columns: new[] { "sede_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipos_ambiente_id",
                schema: "recursos",
                table: "equipos",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipos_codigo_patrimonial",
                schema: "recursos",
                table: "equipos",
                column: "codigo_patrimonial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipos_dependencia_id",
                schema: "recursos",
                table: "equipos",
                column: "dependencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_franjas_horarias_dia_semana_hora_inicio_hora_fin",
                schema: "seguridad",
                table: "franjas_horarias",
                columns: new[] { "dia_semana", "hora_inicio", "hora_fin" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_historial_tipo_ambiente_ambiente_id",
                schema: "recursos",
                table: "historial_tipo_ambiente",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_ambiente_ambiente_id",
                schema: "reservas",
                table: "historial_uso_ambiente",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_ambiente_reserva_id",
                schema: "reservas",
                table: "historial_uso_ambiente",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_ambiente_usuario_id",
                schema: "reservas",
                table: "historial_uso_ambiente",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_equipo_ambiente_id",
                schema: "recursos",
                table: "historial_uso_equipo",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_equipo_equipo_id",
                schema: "recursos",
                table: "historial_uso_equipo",
                column: "equipo_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_equipo_usuario_id",
                schema: "recursos",
                table: "historial_uso_equipo",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_permisos_codigo",
                schema: "seguridad",
                table: "permisos",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pisos_edificio_id_numero",
                schema: "estructura",
                table: "pisos",
                columns: new[] { "edificio_id", "numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prioridades_reserva_rol_id",
                schema: "seguridad",
                table: "prioridades_reserva",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_ambiente_id",
                schema: "reservas",
                table: "reservas",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_dependencia_solicitante_id",
                schema: "reservas",
                table: "reservas",
                column: "dependencia_solicitante_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_solicitante_id",
                schema: "reservas",
                table: "reservas",
                column: "solicitante_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_calendario_dia_reserva_id_fecha_hora_inicio_hora_f~",
                schema: "reservas",
                table: "reservas_calendario_dia",
                columns: new[] { "reserva_id", "fecha", "hora_inicio", "hora_fin" });

            migrationBuilder.CreateIndex(
                name: "IX_roles_codigo",
                schema: "usuario",
                table: "roles",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_permisos_permiso_id",
                schema: "seguridad",
                table: "roles_permisos",
                column: "permiso_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_permisos_rol_id_permiso_id",
                schema: "seguridad",
                table: "roles_permisos",
                columns: new[] { "rol_id", "permiso_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sedes_codigo",
                schema: "estructura",
                table: "sedes",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_correo",
                schema: "usuario",
                table: "usuarios",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_dependencia_id",
                schema: "usuario",
                table: "usuarios",
                column: "dependencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_nombre_usuario",
                schema: "usuario",
                table: "usuarios",
                column: "nombre_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_roles_rol_id",
                schema: "usuario",
                table: "usuarios_roles",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_roles_usuario_id_rol_id_vigencia_desde",
                schema: "usuario",
                table: "usuarios_roles",
                columns: new[] { "usuario_id", "rol_id", "vigencia_desde" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bitacora_auditoria",
                schema: "auditoria");

            migrationBuilder.DropTable(
                name: "bloqueos_ambiente",
                schema: "recursos");

            migrationBuilder.DropTable(
                name: "disponibilidades_ambiente",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "historial_configuracion",
                schema: "auditoria");

            migrationBuilder.DropTable(
                name: "historial_login",
                schema: "auditoria");

            migrationBuilder.DropTable(
                name: "historial_movimiento_global",
                schema: "auditoria");

            migrationBuilder.DropTable(
                name: "historial_tipo_ambiente",
                schema: "recursos");

            migrationBuilder.DropTable(
                name: "historial_uso_ambiente",
                schema: "reservas");

            migrationBuilder.DropTable(
                name: "historial_uso_equipo",
                schema: "recursos");

            migrationBuilder.DropTable(
                name: "prioridades_reserva",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "reservas_calendario_dia",
                schema: "reservas");

            migrationBuilder.DropTable(
                name: "roles_permisos",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "usuarios_roles",
                schema: "usuario");

            migrationBuilder.DropTable(
                name: "franjas_horarias",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "equipos",
                schema: "recursos");

            migrationBuilder.DropTable(
                name: "reservas",
                schema: "reservas");

            migrationBuilder.DropTable(
                name: "permisos",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "usuario");

            migrationBuilder.DropTable(
                name: "ambientes",
                schema: "estructura");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "usuario");

            migrationBuilder.DropTable(
                name: "pisos",
                schema: "estructura");

            migrationBuilder.DropTable(
                name: "dependencias",
                schema: "estructura");

            migrationBuilder.DropTable(
                name: "edificios",
                schema: "estructura");

            migrationBuilder.DropTable(
                name: "sedes",
                schema: "estructura");
        }
    }
}
