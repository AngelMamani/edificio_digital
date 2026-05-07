using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("ambientes", Schema = "estructura")]
public class Ambiente
{
    [Key]
    public Guid Id { get; set; }

    [Column("piso_id")]
    public Guid PisoId { get; set; }

    [Column("dependencia_id")]
    public Guid DependenciaId { get; set; }

    [Column("codigo")]
    [MaxLength(50)]
    public string Codigo { get; set; } = string.Empty;

    [Column("nombre")]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Column("tipo")]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Column("aforo_maximo")]
    public int AforoMaximo { get; set; }

    [Column("estado")]
    [MaxLength(50)]
    public string Estado { get; set; } = string.Empty;

    [Column("publicable")]
    public bool Publicable { get; set; }

    [Column("observacion")]
    [MaxLength(500)]
    public string? Observacion { get; set; }

    public virtual Piso Piso { get; set; } = null!;
    public virtual Dependencia Dependencia { get; set; } = null!;
    public virtual ICollection<Equipo> Equipos { get; set; } = [];
    public virtual ICollection<HistorialTipoAmbiente> HistorialTipos { get; set; } = [];
    public virtual ICollection<BloqueoAmbiente> Bloqueos { get; set; } = [];
    public virtual ICollection<HistorialUsoEquipo> HistorialUsoEquipos { get; set; } = [];
    public virtual ICollection<DisponibilidadAmbiente> Disponibilidades { get; set; } = [];
    public virtual ICollection<Reserva> Reservas { get; set; } = [];
    public virtual ICollection<HistorialUsoAmbiente> HistorialUsoAmbientes { get; set; } = [];
}
