using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model.Usuario;

[Table("roles", Schema = "usuario")]
public class Rol
{
    [Key]
    public Guid Id { get; set; }

    [Column("codigo")]
    [MaxLength(100)]
    public string Codigo { get; set; } = string.Empty;

    [Column("nombre")]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    public virtual ICollection<UsuarioRol> UsuarioRoles { get; set; } = [];
}
