using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edificio_digital.Entity.Migrations
{
    /// <inheritdoc />
    public partial class InicialDerCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_usuarios_roles_usuario_id",
                schema: "usuario",
                table: "usuarios_roles");

            migrationBuilder.AddColumn<DateTime>(
                name: "vigencia_desde",
                schema: "usuario",
                table: "usuarios_roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "vigencia_hasta",
                schema: "usuario",
                table: "usuarios_roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "dependencia_id",
                schema: "usuario",
                table: "usuarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "bitacora_auditoria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tabla = table.Column<string>(type: "text", nullable: false),
                    registro_id = table.Column<string>(type: "text", nullable: false),
                    accion = table.Column<string>(type: "text", nullable: false),
                    valores_anteriores_json = table.Column<string>(type: "text", nullable: true),
                    valores_nuevos_json = table.Column<string>(type: "text", nullable: true),
                    direccion_ip = table.Column<string>(type: "text", nullable: true),
                    agente_usuario = table.Column<string>(type: "text", nullable: true),
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
                name: "franjas_horarias",
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
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modulo = table.Column<string>(type: "text", nullable: false),
                    clave_configuracion = table.Column<string>(type: "text", nullable: false),
                    valor_anterior = table.Column<string>(type: "text", nullable: true),
                    valor_nuevo = table.Column<string>(type: "text", nullable: true),
                    accion = table.Column<string>(type: "text", nullable: false),
                    ejecutado_por = table.Column<string>(type: "text", nullable: true),
                    observacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_configuracion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "historial_login",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    usuario_intentado = table.Column<string>(type: "text", nullable: false),
                    exitoso = table.Column<bool>(type: "boolean", nullable: false),
                    causa = table.Column<string>(type: "text", nullable: true),
                    detalle_error = table.Column<string>(type: "text", nullable: true),
                    direccion_ip = table.Column<string>(type: "text", nullable: true),
                    agente_usuario = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_login", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "historial_movimiento_global",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha_hora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    entidad = table.Column<string>(type: "text", nullable: false),
                    entidad_id = table.Column<string>(type: "text", nullable: true),
                    accion = table.Column<string>(type: "text", nullable: false),
                    creado_por = table.Column<string>(type: "text", nullable: true),
                    modificado_por = table.Column<string>(type: "text", nullable: true),
                    eliminado_por = table.Column<string>(type: "text", nullable: true),
                    motivo = table.Column<string>(type: "text", nullable: true),
                    detalle_json = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_movimiento_global", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permisos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    recurso = table.Column<string>(type: "text", nullable: false),
                    accion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permisos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prioridades_reserva",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tipo_usuario = table.Column<string>(type: "text", nullable: true),
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
                name: "sedes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    direccion = table.Column<string>(type: "text", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sedes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles_permisos",
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
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sede_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dependencias", x => x.id);
                    table.ForeignKey(
                        name: "FK_dependencias_sedes_sede_id",
                        column: x => x.sede_id,
                        principalTable: "sedes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "edificios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sede_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_edificios", x => x.id);
                    table.ForeignKey(
                        name: "FK_edificios_sedes_sede_id",
                        column: x => x.sede_id,
                        principalTable: "sedes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pisos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    edificio_id = table.Column<Guid>(type: "uuid", nullable: false),
                    numero = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pisos", x => x.id);
                    table.ForeignKey(
                        name: "FK_pisos_edificios_edificio_id",
                        column: x => x.edificio_id,
                        principalTable: "edificios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ambientes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    piso_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dependencia_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: false),
                    aforo_maximo = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false),
                    publicable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    observacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ambientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_ambientes_dependencias_dependencia_id",
                        column: x => x.dependencia_id,
                        principalTable: "dependencias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ambientes_pisos_piso_id",
                        column: x => x.piso_id,
                        principalTable: "pisos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bloqueos_ambiente",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bloqueado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    motivo_bloqueo = table.Column<string>(type: "text", nullable: false),
                    bloqueado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    desbloqueado_por_usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    motivo_desbloqueo = table.Column<string>(type: "text", nullable: true),
                    desbloqueado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bloqueos_ambiente", x => x.id);
                    table.ForeignKey(
                        name: "FK_bloqueos_ambiente_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
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
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_disponibilidades_ambiente_franjas_horarias_franja_horaria_id",
                        column: x => x.franja_horaria_id,
                        principalTable: "franjas_horarias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "equipos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dependencia_id = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo_patrimonial = table.Column<string>(type: "text", nullable: false),
                    serie = table.Column<string>(type: "text", nullable: true),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipos", x => x.id);
                    table.ForeignKey(
                        name: "FK_equipos_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_equipos_dependencias_dependencia_id",
                        column: x => x.dependencia_id,
                        principalTable: "dependencias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "historial_tipo_ambiente",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_anterior = table.Column<string>(type: "text", nullable: false),
                    tipo_nuevo = table.Column<string>(type: "text", nullable: false),
                    motivo_cambio = table.Column<string>(type: "text", nullable: false),
                    fecha_cambio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_tipo_ambiente", x => x.id);
                    table.ForeignKey(
                        name: "FK_historial_tipo_ambiente_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    solicitante_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dependencia_solicitante_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tipo_reserva = table.Column<string>(type: "text", nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    hora_fin = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    tipo_franja = table.Column<string>(type: "text", nullable: false),
                    regla_recurrencia = table.Column<string>(type: "text", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: false),
                    prioridad_aplicada = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    motivo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservas_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservas_dependencias_dependencia_solicitante_id",
                        column: x => x.dependencia_solicitante_id,
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
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    motivo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_uso_equipo", x => x.id);
                    table.ForeignKey(
                        name: "FK_historial_uso_equipo_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_historial_uso_equipo_equipos_equipo_id",
                        column: x => x.equipo_id,
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
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ambiente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    reserva_id = table.Column<Guid>(type: "uuid", nullable: true),
                    inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tipo_uso = table.Column<string>(type: "text", nullable: false),
                    observacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_uso_ambiente", x => x.id);
                    table.ForeignKey(
                        name: "FK_historial_uso_ambiente_ambientes_ambiente_id",
                        column: x => x.ambiente_id,
                        principalTable: "ambientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_historial_uso_ambiente_reservas_reserva_id",
                        column: x => x.reserva_id,
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
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reserva_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    hora_inicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    hora_fin = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas_calendario_dia", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservas_calendario_dia_reservas_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_roles_usuario_id_rol_id_vigencia_desde",
                schema: "usuario",
                table: "usuarios_roles",
                columns: new[] { "usuario_id", "rol_id", "vigencia_desde" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_dependencia_id",
                schema: "usuario",
                table: "usuarios",
                column: "dependencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_ambientes_dependencia_id",
                table: "ambientes",
                column: "dependencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_ambientes_piso_id_codigo",
                table: "ambientes",
                columns: new[] { "piso_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bitacora_auditoria_actor_usuario_id",
                table: "bitacora_auditoria",
                column: "actor_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_bloqueos_ambiente_ambiente_id",
                table: "bloqueos_ambiente",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_bloqueos_ambiente_bloqueado_por_usuario_id",
                table: "bloqueos_ambiente",
                column: "bloqueado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_bloqueos_ambiente_desbloqueado_por_usuario_id",
                table: "bloqueos_ambiente",
                column: "desbloqueado_por_usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_dependencias_sede_id_codigo",
                table: "dependencias",
                columns: new[] { "sede_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidades_ambiente_ambiente_id",
                table: "disponibilidades_ambiente",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_disponibilidades_ambiente_franja_horaria_id",
                table: "disponibilidades_ambiente",
                column: "franja_horaria_id");

            migrationBuilder.CreateIndex(
                name: "IX_edificios_sede_id_codigo",
                table: "edificios",
                columns: new[] { "sede_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipos_ambiente_id",
                table: "equipos",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipos_codigo_patrimonial",
                table: "equipos",
                column: "codigo_patrimonial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipos_dependencia_id",
                table: "equipos",
                column: "dependencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_franjas_horarias_dia_semana_hora_inicio_hora_fin",
                table: "franjas_horarias",
                columns: new[] { "dia_semana", "hora_inicio", "hora_fin" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_historial_tipo_ambiente_ambiente_id",
                table: "historial_tipo_ambiente",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_ambiente_ambiente_id",
                table: "historial_uso_ambiente",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_ambiente_reserva_id",
                table: "historial_uso_ambiente",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_ambiente_usuario_id",
                table: "historial_uso_ambiente",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_equipo_ambiente_id",
                table: "historial_uso_equipo",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_equipo_equipo_id",
                table: "historial_uso_equipo",
                column: "equipo_id");

            migrationBuilder.CreateIndex(
                name: "IX_historial_uso_equipo_usuario_id",
                table: "historial_uso_equipo",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_permisos_codigo",
                table: "permisos",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pisos_edificio_id_numero",
                table: "pisos",
                columns: new[] { "edificio_id", "numero" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prioridades_reserva_rol_id",
                table: "prioridades_reserva",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_ambiente_id",
                table: "reservas",
                column: "ambiente_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_dependencia_solicitante_id",
                table: "reservas",
                column: "dependencia_solicitante_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_solicitante_id",
                table: "reservas",
                column: "solicitante_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_calendario_dia_reserva_id_fecha_hora_inicio_hora_f~",
                table: "reservas_calendario_dia",
                columns: new[] { "reserva_id", "fecha", "hora_inicio", "hora_fin" });

            migrationBuilder.CreateIndex(
                name: "IX_roles_permisos_permiso_id",
                table: "roles_permisos",
                column: "permiso_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_permisos_rol_id_permiso_id",
                table: "roles_permisos",
                columns: new[] { "rol_id", "permiso_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sedes_codigo",
                table: "sedes",
                column: "codigo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_usuarios_dependencias_dependencia_id",
                schema: "usuario",
                table: "usuarios",
                column: "dependencia_id",
                principalTable: "dependencias",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuarios_dependencias_dependencia_id",
                schema: "usuario",
                table: "usuarios");

            migrationBuilder.DropTable(
                name: "bitacora_auditoria");

            migrationBuilder.DropTable(
                name: "bloqueos_ambiente");

            migrationBuilder.DropTable(
                name: "disponibilidades_ambiente");

            migrationBuilder.DropTable(
                name: "historial_configuracion");

            migrationBuilder.DropTable(
                name: "historial_login");

            migrationBuilder.DropTable(
                name: "historial_movimiento_global");

            migrationBuilder.DropTable(
                name: "historial_tipo_ambiente");

            migrationBuilder.DropTable(
                name: "historial_uso_ambiente");

            migrationBuilder.DropTable(
                name: "historial_uso_equipo");

            migrationBuilder.DropTable(
                name: "prioridades_reserva");

            migrationBuilder.DropTable(
                name: "reservas_calendario_dia");

            migrationBuilder.DropTable(
                name: "roles_permisos");

            migrationBuilder.DropTable(
                name: "franjas_horarias");

            migrationBuilder.DropTable(
                name: "equipos");

            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "permisos");

            migrationBuilder.DropTable(
                name: "ambientes");

            migrationBuilder.DropTable(
                name: "dependencias");

            migrationBuilder.DropTable(
                name: "pisos");

            migrationBuilder.DropTable(
                name: "edificios");

            migrationBuilder.DropTable(
                name: "sedes");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_roles_usuario_id_rol_id_vigencia_desde",
                schema: "usuario",
                table: "usuarios_roles");

            migrationBuilder.DropIndex(
                name: "IX_usuarios_dependencia_id",
                schema: "usuario",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "vigencia_desde",
                schema: "usuario",
                table: "usuarios_roles");

            migrationBuilder.DropColumn(
                name: "vigencia_hasta",
                schema: "usuario",
                table: "usuarios_roles");

            migrationBuilder.DropColumn(
                name: "dependencia_id",
                schema: "usuario",
                table: "usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_roles_usuario_id",
                schema: "usuario",
                table: "usuarios_roles",
                column: "usuario_id");
        }
    }
}
