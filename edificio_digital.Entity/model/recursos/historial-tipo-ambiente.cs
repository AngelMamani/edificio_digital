using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("historial_tipo_ambiente", Schema = "recursos")]
public class HistorialTipoAmbiente
{
    [Key]
    public Guid Id { get; set; }

    [Column("ambiente_id")]
    public Guid AmbienteId { get; set; }

    [Column("tipo_anterior")]
    [MaxLength(50)]
    public string TipoAnterior { get; set; } = string.Empty;

    [Column("tipo_nuevo")]
    [MaxLength(50)]
    public string TipoNuevo { get; set; } = string.Empty;

    [Column("motivo_cambio")]
    [MaxLength(200)]
    public string MotivoCambio { get; set; } = string.Empty;

    [Column("fecha_cambio")]
    public DateTime FechaCambio { get; set; }

    public virtual Ambiente Ambiente { get; set; } = null!;
}
