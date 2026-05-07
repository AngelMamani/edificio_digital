using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("historial_login", Schema = "auditoria")]
public class HistorialLogin
{
    [Key]
    public Guid Id { get; set; }

    [Column("fecha_hora")]
    public DateTime FechaHora { get; set; }

    [Column("usuario_intentado")]
    [MaxLength(200)]
    public string UsuarioIntentado { get; set; } = string.Empty;

    [Column("exitoso")]
    public bool Exitoso { get; set; }

    [Column("causa")]
    [MaxLength(250)]
    public string? Causa { get; set; }

    [Column("detalle_error")]
    public string? DetalleError { get; set; }

    [Column("direccion_ip")]
    [MaxLength(50)]
    public string? DireccionIp { get; set; }

    [Column("agente_usuario")]
    [MaxLength(250)]
    public string? AgenteUsuario { get; set; }
}
