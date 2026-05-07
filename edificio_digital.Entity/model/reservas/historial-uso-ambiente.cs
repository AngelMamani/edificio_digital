using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edificio_digital.Entity.Model.Usuario;

namespace edificio_digital.Entity.Model;

[Table("historial_uso_ambiente", Schema = "reservas")]
public class HistorialUsoAmbiente
{
    [Key]
    public Guid Id { get; set; }

    [Column("ambiente_id")]
    public Guid AmbienteId { get; set; }

    [Column("usuario_id")]
    public Guid? UsuarioId { get; set; }

    [Column("reserva_id")]
    public Guid? ReservaId { get; set; }

    [Column("inicio")]
    public DateTime Inicio { get; set; }

    [Column("fin")]
    public DateTime? Fin { get; set; }

    [Column("tipo_uso")]
    [MaxLength(50)]
    public string TipoUso { get; set; } = string.Empty;

    [Column("observacion")]
    [MaxLength(500)]
    public string? Observacion { get; set; }

    public virtual Ambiente Ambiente { get; set; } = null!;
    public virtual edificio_digital.Entity.Model.Usuario.Usuario? Usuario { get; set; }
    public virtual Reserva? Reserva { get; set; }
}
