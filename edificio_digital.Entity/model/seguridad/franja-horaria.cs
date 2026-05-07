using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("franjas_horarias", Schema = "seguridad")]
public class FranjaHoraria
{
    [Key]
    public Guid Id { get; set; }

    [Column("dia_semana")]
    public int DiaSemana { get; set; }

    [Column("hora_inicio")]
    public TimeOnly HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly HoraFin { get; set; }

    public virtual ICollection<DisponibilidadAmbiente> Disponibilidades { get; set; } = [];
}
