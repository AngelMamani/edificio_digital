using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edificio_digital.Entity.Model.Usuario;

namespace edificio_digital.Entity.Model;

[Table("prioridades_reserva", Schema = "seguridad")]
public class PrioridadReserva
{
    [Key]
    public Guid Id { get; set; }

    [Column("rol_id")]
    public Guid? RolId { get; set; }

    [Column("tipo_usuario")]
    [MaxLength(50)]
    public string? TipoUsuario { get; set; }

    [Column("nivel_prioridad")]
    public int NivelPrioridad { get; set; }

    [Column("antelacion_horas")]
    public int AntelacionHoras { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    public virtual Rol? Rol { get; set; }
}
