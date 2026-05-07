using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("disponibilidades_ambiente", Schema = "seguridad")]
public class DisponibilidadAmbiente
{
    [Key]
    public Guid Id { get; set; }

    [Column("ambiente_id")]
    public Guid AmbienteId { get; set; }

    [Column("franja_horaria_id")]
    public Guid FranjaHorariaId { get; set; }

    [Column("vigencia_desde")]
    public DateOnly VigenciaDesde { get; set; }

    [Column("vigencia_hasta")]
    public DateOnly? VigenciaHasta { get; set; }

    [Column("disponible")]
    public bool Disponible { get; set; } = true;

    public virtual Ambiente Ambiente { get; set; } = null!;
    public virtual FranjaHoraria FranjaHoraria { get; set; } = null!;
}
