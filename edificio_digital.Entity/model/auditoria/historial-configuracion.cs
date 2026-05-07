using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("historial_configuracion", Schema = "auditoria")]
public class HistorialConfiguracion
{
    [Key]
    public Guid Id { get; set; }

    [Column("fecha_hora")]
    public DateTime FechaHora { get; set; }

    [Column("modulo")]
    [MaxLength(100)]
    public string Modulo { get; set; } = string.Empty;

    [Column("clave_configuracion")]
    [MaxLength(150)]
    public string ClaveConfiguracion { get; set; } = string.Empty;

    [Column("valor_anterior")]
    public string? ValorAnterior { get; set; }

    [Column("valor_nuevo")]
    public string? ValorNuevo { get; set; }

    [Column("accion")]
    [MaxLength(50)]
    public string Accion { get; set; } = string.Empty;

    [Column("ejecutado_por")]
    [MaxLength(150)]
    public string? EjecutadoPor { get; set; }

    [Column("observacion")]
    [MaxLength(500)]
    public string? Observacion { get; set; }
}
