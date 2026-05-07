using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edificio_digital.Entity.Model.Usuario;

namespace edificio_digital.Entity.Model;

[Table("reservas", Schema = "reservas")]
public class Reserva
{
    [Key]
    public Guid Id { get; set; }

    [Column("ambiente_id")]
    public Guid AmbienteId { get; set; }

    [Column("solicitante_id")]
    public Guid SolicitanteId { get; set; }

    [Column("dependencia_solicitante_id")]
    public Guid? DependenciaSolicitanteId { get; set; }

    [Column("tipo_reserva")]
    [MaxLength(50)]
    public string TipoReserva { get; set; } = string.Empty;

    [Column("fecha_inicio")]
    public DateOnly FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateOnly FechaFin { get; set; }

    [Column("hora_inicio")]
    public TimeOnly? HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly? HoraFin { get; set; }

    [Column("tipo_franja")]
    [MaxLength(50)]
    public string TipoFranja { get; set; } = string.Empty;

    [Column("regla_recurrencia")]
    [MaxLength(200)]
    public string? ReglaRecurrencia { get; set; }

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; } = string.Empty;

    [Column("prioridad_aplicada")]
    public int PrioridadAplicada { get; set; }

    [Column("motivo")]
    [MaxLength(250)]
    public string Motivo { get; set; } = string.Empty;

    public virtual Ambiente Ambiente { get; set; } = null!;
    public virtual edificio_digital.Entity.Model.Usuario.Usuario Solicitante { get; set; } = null!;
    public virtual Dependencia? DependenciaSolicitante { get; set; }
    public virtual ICollection<ReservaCalendarioDia> CalendarioDias { get; set; } = [];
    public virtual ICollection<HistorialUsoAmbiente> HistorialUsoAmbientes { get; set; } = [];
}
