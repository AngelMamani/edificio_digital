using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace edificio_digital.Models.Models.Usuarios
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Usuario";
        public bool Activo { get; set; } = true;
        public Guid? DependenciaId { get; set; }
        public string Contrasena { get; set; } = string.Empty;
    }
}
