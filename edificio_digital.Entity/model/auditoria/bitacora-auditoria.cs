using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edificio_digital.Entity.Model.Usuario;

namespace edificio_digital.Entity.Model;

[Table("bitacora_auditoria", Schema = "auditoria")]
public class BitacoraAuditoria
{
    [Key]
    public Guid Id { get; set; }

    [Column("fecha_hora")]
    public DateTime FechaHora { get; set; }

    [Column("tabla")]
    [MaxLength(100)]
    public string Tabla { get; set; } = string.Empty;

    [Column("registro_id")]
    [MaxLength(100)]
    public string RegistroId { get; set; } = string.Empty;

    [Column("accion")]
    [MaxLength(50)]
    public string Accion { get; set; } = string.Empty;

    [Column("valores_anteriores_json", TypeName = "jsonb")]
    public string? ValoresAnterioresJson { get; set; }

    [Column("valores_nuevos_json", TypeName = "jsonb")]
    public string? ValoresNuevosJson { get; set; }

    [Column("direccion_ip")]
    [MaxLength(50)]
    public string? DireccionIp { get; set; }

    [Column("agente_usuario")]
    [MaxLength(250)]
    public string? AgenteUsuario { get; set; }

    [Column("actor_usuario_id")]
    public Guid? ActorUsuarioId { get; set; }

    public virtual edificio_digital.Entity.Model.Usuario.Usuario? ActorUsuario { get; set; }
}
