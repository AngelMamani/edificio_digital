using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("equipos", Schema = "recursos")]
public class Equipo
{
    [Key]
    public Guid Id { get; set; }

    [Column("ambiente_id")]
    public Guid AmbienteId { get; set; }

    [Column("dependencia_id")]
    public Guid DependenciaId { get; set; }

    [Column("codigo_patrimonial")]
    [MaxLength(100)]
    public string CodigoPatrimonial { get; set; } = string.Empty;

    [Column("serie")]
    [MaxLength(100)]
    public string? Serie { get; set; }

    [Column("nombre")]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Column("tipo")]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; } = string.Empty;

    public virtual Ambiente Ambiente { get; set; } = null!;
    public virtual Dependencia Dependencia { get; set; } = null!;
    public virtual ICollection<HistorialUsoEquipo> HistorialUso { get; set; } = [];
}
