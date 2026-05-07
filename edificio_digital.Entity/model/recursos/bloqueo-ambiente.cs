using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edificio_digital.Entity.Model.Usuario;

namespace edificio_digital.Entity.Model;

[Table("bloqueos_ambiente", Schema = "recursos")]
public class BloqueoAmbiente
{
    [Key]
    public Guid Id { get; set; }

    [Column("ambiente_id")]
    public Guid AmbienteId { get; set; }

    [Column("bloqueado_por_usuario_id")]
    public Guid BloqueadoPorUsuarioId { get; set; }

    [Column("motivo_bloqueo")]
    [MaxLength(250)]
    public string MotivoBloqueo { get; set; } = string.Empty;

    [Column("bloqueado_en")]
    public DateTime BloqueadoEn { get; set; }

    [Column("desbloqueado_por_usuario_id")]
    public Guid? DesbloqueadoPorUsuarioId { get; set; }

    [Column("motivo_desbloqueo")]
    [MaxLength(250)]
    public string? MotivoDesbloqueo { get; set; }

    [Column("desbloqueado_en")]
    public DateTime? DesbloqueadoEn { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    public virtual Ambiente Ambiente { get; set; } = null!;
    public virtual edificio_digital.Entity.Model.Usuario.Usuario BloqueadoPorUsuario { get; set; } = null!;
    public virtual edificio_digital.Entity.Model.Usuario.Usuario? DesbloqueadoPorUsuario { get; set; }
}
