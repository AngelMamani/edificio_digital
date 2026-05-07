using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edificio_digital.Entity.Migrations
{
    /// <inheritdoc />
    public partial class SemillaUsuariosEjemplo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Mismos correos que InMemoryAuthRepository; contraseña de prueba: 123456 (texto plano, solo desarrollo).
            migrationBuilder.InsertData(
                schema: "usuario",
                table: "usuarios",
                columns: ["id", "username", "email", "nombre_completo", "tipo", "activo", "password"],
                values: new object[,]
                {
                    {
                        new Guid("a0000000-0000-4000-8000-000000000001"), "admin",
                        "admin@edificiodigital.com", "Usuario Administrador (ejemplo)", "Administrador", true, "123456"
                    },
                    {
                        new Guid("a0000000-0000-4000-8000-000000000002"), "docente",
                        "docente@edificiodigital.com", "Usuario Docente (ejemplo)", "Docente", true, "123456"
                    },
                    {
                        new Guid("a0000000-0000-4000-8000-000000000003"), "alumno",
                        "alumno@edificiodigital.com", "Usuario Alumno (ejemplo)", "Alumno", true, "123456"
                    }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "usuario",
                table: "usuarios",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-4000-8000-000000000001"));

            migrationBuilder.DeleteData(
                schema: "usuario",
                table: "usuarios",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-4000-8000-000000000002"));

            migrationBuilder.DeleteData(
                schema: "usuario",
                table: "usuarios",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-4000-8000-000000000003"));
        }
    }
}
