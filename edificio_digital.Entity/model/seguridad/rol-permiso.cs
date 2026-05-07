using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edificio_digital.Entity.Model.Usuario;

namespace edificio_digital.Entity.Model;

[Table("roles_permisos", Schema = "seguridad")]
public class RolPermiso
{
    [Key]
    public Guid Id { get; set; }

    [Column("rol_id")]
    public Guid RolId { get; set; }

    [Column("permiso_id")]
    public Guid PermisoId { get; set; }

    public virtual Rol Rol { get; set; } = null!;
    public virtual Permiso Permiso { get; set; } = null!;
}
