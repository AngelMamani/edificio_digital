using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("historial_movimiento_global", Schema = "auditoria")]
public class HistorialMovimientoGlobal
{
    [Key]
    public Guid Id { get; set; }

    [Column("fecha_hora")]
    public DateTime FechaHora { get; set; }

    [Column("entidad")]
    [MaxLength(100)]
    public string Entidad { get; set; } = string.Empty;

    [Column("entidad_id")]
    [MaxLength(100)]
    public string? EntidadId { get; set; }

    [Column("accion")]
    [MaxLength(50)]
    public string Accion { get; set; } = string.Empty;

    [Column("creado_por")]
    [MaxLength(150)]
    public string? CreadoPor { get; set; }

    [Column("modificado_por")]
    [MaxLength(150)]
    public string? ModificadoPor { get; set; }

    [Column("eliminado_por")]
    [MaxLength(150)]
    public string? EliminadoPor { get; set; }

    [Column("motivo")]
    [MaxLength(250)]
    public string? Motivo { get; set; }

    [Column("detalle_json", TypeName = "jsonb")]
    public string? DetalleJson { get; set; }
}
