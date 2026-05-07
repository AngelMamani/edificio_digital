using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edificio_digital.Entity.Model.Usuario;

namespace edificio_digital.Entity.Model;

[Table("historial_uso_equipo", Schema = "recursos")]
public class HistorialUsoEquipo
{
    [Key]
    public Guid Id { get; set; }

    [Column("equipo_id")]
    public Guid EquipoId { get; set; }

    [Column("ambiente_id")]
    public Guid AmbienteId { get; set; }

    [Column("usuario_id")]
    public Guid? UsuarioId { get; set; }

    [Column("inicio")]
    public DateTime Inicio { get; set; }

    [Column("fin")]
    public DateTime? Fin { get; set; }

    [Column("motivo")]
    [MaxLength(250)]
    public string? Motivo { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;
    public virtual Ambiente Ambiente { get; set; } = null!;
    public virtual edificio_digital.Entity.Model.Usuario.Usuario? Usuario { get; set; }
}
