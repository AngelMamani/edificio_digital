using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("reservas_calendario_dia", Schema = "reservas")]
public class ReservaCalendarioDia
{
    [Key]
    public Guid Id { get; set; }

    [Column("reserva_id")]
    public Guid ReservaId { get; set; }

    [Column("fecha")]
    public DateOnly Fecha { get; set; }

    [Column("hora_inicio")]
    public TimeOnly HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly HoraFin { get; set; }

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; } = string.Empty;

    public virtual Reserva Reserva { get; set; } = null!;
}
