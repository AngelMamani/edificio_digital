using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model.Usuario;

[Table("usuarios_roles", Schema = "usuario")]
public class UsuarioRol
{
    [Key]
    public Guid Id { get; set; }

    [Column("usuario_id")]
    public Guid UsuarioId { get; set; }

    [Column("rol_id")]
    public Guid RolId { get; set; }

    [Column("vigencia_desde")]
    public DateTime VigenciaDesde { get; set; }

    [Column("vigencia_hasta")]
    public DateTime? VigenciaHasta { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Rol Rol { get; set; } = null!;
}
