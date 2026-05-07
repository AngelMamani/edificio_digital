using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edificio_digital.Entity.Migrations
{
    /// <inheritdoc />
    public partial class AjusteEspanolColumnasUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "sedes",
                newName: "sedes",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "roles_permisos",
                newName: "roles_permisos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "reservas_calendario_dia",
                newName: "reservas_calendario_dia",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "reservas",
                newName: "reservas",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "prioridades_reserva",
                newName: "prioridades_reserva",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "pisos",
                newName: "pisos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "permisos",
                newName: "permisos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_uso_equipo",
                newName: "historial_uso_equipo",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_uso_ambiente",
                newName: "historial_uso_ambiente",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_tipo_ambiente",
                newName: "historial_tipo_ambiente",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_movimiento_global",
                newName: "historial_movimiento_global",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_login",
                newName: "historial_login",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "historial_configuracion",
                newName: "historial_configuracion",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "franjas_horarias",
                newName: "franjas_horarias",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "equipos",
                newName: "equipos",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "edificios",
                newName: "edificios",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "disponibilidades_ambiente",
                newName: "disponibilidades_ambiente",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "dependencias",
                newName: "dependencias",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "bloqueos_ambiente",
                newName: "bloqueos_ambiente",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "bitacora_auditoria",
                newName: "bitacora_auditoria",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "ambientes",
                newName: "ambientes",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "username",
                schema: "usuario",
                table: "usuarios",
                newName: "nombre_usuario");

            migrationBuilder.RenameColumn(
                name: "password",
                schema: "usuario",
                table: "usuarios",
                newName: "contrasena");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "usuario",
                table: "usuarios",
                newName: "correo");

            migrationBuilder.RenameIndex(
                name: "IX_usuarios_username",
                schema: "usuario",
                table: "usuarios",
                newName: "IX_usuarios_nombre_usuario");

            migrationBuilder.RenameIndex(
                name: "IX_usuarios_email",
                schema: "usuario",
                table: "usuarios",
                newName: "IX_usuarios_correo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "sedes",
                schema: "public",
                newName: "sedes");

            migrationBuilder.RenameTable(
                name: "roles_permisos",
                schema: "public",
                newName: "roles_permisos");

            migrationBuilder.RenameTable(
                name: "reservas_calendario_dia",
                schema: "public",
                newName: "reservas_calendario_dia");

            migrationBuilder.RenameTable(
                name: "reservas",
                schema: "public",
                newName: "reservas");

            migrationBuilder.RenameTable(
                name: "prioridades_reserva",
                schema: "public",
                newName: "prioridades_reserva");

            migrationBuilder.RenameTable(
                name: "pisos",
                schema: "public",
                newName: "pisos");

            migrationBuilder.RenameTable(
                name: "permisos",
                schema: "public",
                newName: "permisos");

            migrationBuilder.RenameTable(
                name: "historial_uso_equipo",
                schema: "public",
                newName: "historial_uso_equipo");

            migrationBuilder.RenameTable(
                name: "historial_uso_ambiente",
                schema: "public",
                newName: "historial_uso_ambiente");

            migrationBuilder.RenameTable(
                name: "historial_tipo_ambiente",
                schema: "public",
                newName: "historial_tipo_ambiente");

            migrationBuilder.RenameTable(
                name: "historial_movimiento_global",
                schema: "public",
                newName: "historial_movimiento_global");

            migrationBuilder.RenameTable(
                name: "historial_login",
                schema: "public",
                newName: "historial_login");

            migrationBuilder.RenameTable(
                name: "historial_configuracion",
                schema: "public",
                newName: "historial_configuracion");

            migrationBuilder.RenameTable(
                name: "franjas_horarias",
                schema: "public",
                newName: "franjas_horarias");

            migrationBuilder.RenameTable(
                name: "equipos",
                schema: "public",
                newName: "equipos");

            migrationBuilder.RenameTable(
                name: "edificios",
                schema: "public",
                newName: "edificios");

            migrationBuilder.RenameTable(
                name: "disponibilidades_ambiente",
                schema: "public",
                newName: "disponibilidades_ambiente");

            migrationBuilder.RenameTable(
                name: "dependencias",
                schema: "public",
                newName: "dependencias");

            migrationBuilder.RenameTable(
                name: "bloqueos_ambiente",
                schema: "public",
                newName: "bloqueos_ambiente");

            migrationBuilder.RenameTable(
                name: "bitacora_auditoria",
                schema: "public",
                newName: "bitacora_auditoria");

            migrationBuilder.RenameTable(
                name: "ambientes",
                schema: "public",
                newName: "ambientes");

            migrationBuilder.RenameColumn(
                name: "nombre_usuario",
                schema: "usuario",
                table: "usuarios",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "correo",
                schema: "usuario",
                table: "usuarios",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "contrasena",
                schema: "usuario",
                table: "usuarios",
                newName: "password");

            migrationBuilder.RenameIndex(
                name: "IX_usuarios_nombre_usuario",
                schema: "usuario",
                table: "usuarios",
                newName: "IX_usuarios_username");

            migrationBuilder.RenameIndex(
                name: "IX_usuarios_correo",
                schema: "usuario",
                table: "usuarios",
                newName: "IX_usuarios_email");
        }
    }
}
