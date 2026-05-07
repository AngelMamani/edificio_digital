using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("permisos", Schema = "seguridad")]
public class Permiso
{
    [Key]
    public Guid Id { get; set; }

    [Column("codigo")]
    [MaxLength(100)]
    public string Codigo { get; set; } = string.Empty;

    [Column("recurso")]
    [MaxLength(100)]
    public string Recurso { get; set; } = string.Empty;

    [Column("accion")]
    [MaxLength(100)]
    public string Accion { get; set; } = string.Empty;

    public virtual ICollection<RolPermiso> RolesPermisos { get; set; } = [];
}
