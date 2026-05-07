using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edificio_digital.Entity.Migrations
{
    /// <inheritdoc />
    public partial class AjusteEsquemasPorModulo : Migration
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

            migrationBuilder.RenameTable(
                name: "sedes",
                schema: "public",
                newName: "sedes",
                newSchema: "estructura");

            migrationBuilder.RenameTable(
                name: "roles_permisos",
                schema: "public",
                newName: "roles_permisos",
                newSchema: "seguridad");

            migrationBuilder.RenameTable(
                name: "reservas_calendario_dia",
                schema: "public",
                newName: "reservas_calendario_dia",
                newSchema: "reservas");

            migrationBuilder.RenameTable(
                name: "reservas",
                schema: "public",
                newName: "reservas",
                newSchema: "reservas");

            migrationBuilder.RenameTable(
                name: "prioridades_reserva",
                schema: "public",
                newName: "prioridades_reserva",
                newSchema: "seguridad");

            migrationBuilder.RenameTable(
                name: "pisos",
                schema: "public",
                newName: "pisos",
                newSchema: "estructura");

            migrationBuilder.RenameTable(
                name: "permisos",
                schema: "public",
                newName: "permisos",
                newSchema: "seguridad");

            migrationBuilder.RenameTable(
                name: "historial_uso_equipo",
                schema: "public",
                newName: "historial_uso_equipo",
                newSchema: "recursos");

            migrationBuilder.RenameTable(
                name: "historial_uso_ambiente",
                schema: "public",
                newName: "historial_uso_ambiente",
                newSchema: "reservas");

            migrationBuilder.RenameTable(
                name: "historial_tipo_ambiente",
                schema: "public",
                newName: "historial_tipo_ambiente",
                newSchema: "recursos");

            migrationBuilder.RenameTable(
                name: "historial_movimiento_global",
                schema: "public",
                newName: "historial_movimiento_global",
                newSchema: "auditoria");

            migrationBuilder.RenameTable(
                name: "historial_login",
                schema: "public",
                newName: "historial_login",
                newSchema: "auditoria");

            migrationBuilder.RenameTable(
                name: "historial_configuracion",
                schema: "public",
                newName: "historial_configuracion",
                newSchema: "auditoria");

            migrationBuilder.RenameTable(
                name: "franjas_horarias",
                schema: "public",
                newName: "franjas_horarias",
                newSchema: "seguridad");

            migrationBuilder.RenameTable(
                name: "equipos",
                schema: "public",
                newName: "equipos",
                newSchema: "recursos");

            migrationBuilder.RenameTable(
                name: "edificios",
                schema: "public",
                newName: "edificios",
                newSchema: "estructura");

            migrationBuilder.RenameTable(
                name: "disponibilidades_ambiente",
                schema: "public",
                newName: "disponibilidades_ambiente",
                newSchema: "seguridad");

            migrationBuilder.RenameTable(
                name: "dependencias",
                schema: "public",
                newName: "dependencias",
                newSchema: "estructura");

            migrationBuilder.RenameTable(
                name: "bloqueos_ambiente",
                schema: "public",
                newName: "bloqueos_ambiente",
                newSchema: "recursos");

            migrationBuilder.RenameTable(
                name: "bitacora_auditoria",
                schema: "public",
                newName: "bitacora_auditoria",
                newSchema: "auditoria");

            migrationBuilder.RenameTable(
                name: "ambientes",
                schema: "public",
                newName: "ambientes",
                newSchema: "estructura");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "sedes",
                schema: "estructura",
                newName: "sedes",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "roles_permisos",
                schema: "seguridad",
                newName: "roles_permisos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "reservas_calendario_dia",
                schema: "reservas",
                newName: "reservas_calendario_dia",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "reservas",
                schema: "reservas",
                newName: "reservas",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "prioridades_reserva",
                schema: "seguridad",
                newName: "prioridades_reserva",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "pisos",
                schema: "estructura",
                newName: "pisos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "permisos",
                schema: "seguridad",
                newName: "permisos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_uso_equipo",
                schema: "recursos",
                newName: "historial_uso_equipo",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_uso_ambiente",
                schema: "reservas",
                newName: "historial_uso_ambiente",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_tipo_ambiente",
                schema: "recursos",
                newName: "historial_tipo_ambiente",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_movimiento_global",
                schema: "auditoria",
                newName: "historial_movimiento_global",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_login",
                schema: "auditoria",
                newName: "historial_login",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_configuracion",
                schema: "auditoria",
                newName: "historial_configuracion",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "franjas_horarias",
                schema: "seguridad",
                newName: "franjas_horarias",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "equipos",
                schema: "recursos",
                newName: "equipos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "edificios",
                schema: "estructura",
                newName: "edificios",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "disponibilidades_ambiente",
                schema: "seguridad",
                newName: "disponibilidades_ambiente",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "dependencias",
                schema: "estructura",
                newName: "dependencias",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "bloqueos_ambiente",
                schema: "recursos",
                newName: "bloqueos_ambiente",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "bitacora_auditoria",
                schema: "auditoria",
                newName: "bitacora_auditoria",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "ambientes",
                schema: "estructura",
                newName: "ambientes",
                newSchema: "public");
        }
    }
}
