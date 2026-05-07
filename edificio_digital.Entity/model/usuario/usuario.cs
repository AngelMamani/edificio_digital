using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model.Usuario
{
    [Table("usuarios", Schema = "usuario")]
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Column("nombre_usuario")]
        [MaxLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Column("correo")]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Column("nombre_completo")]
        [MaxLength(250)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Column("tipo")]
        [MaxLength(50)]
        public string Tipo { get; set; } = "Usuario";

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("dependencia_id")]
        public Guid? DependenciaId { get; set; }

        [Column("contrasena")]
        [MaxLength(250)]
        public string Contrasena { get; set; } = string.Empty;

        public virtual ICollection<UsuarioRol> UsuarioRoles { get; set; } = [];
    }
}