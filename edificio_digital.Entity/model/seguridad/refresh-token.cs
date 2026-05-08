using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model.Seguridad;

[Table("refresh_tokens", Schema = "seguridad")]
public class RefreshTokenEntity
{
    [Key]
    public Guid Id { get; set; }

    [Column("usuario_id")]
    public Guid UsuarioId { get; set; }

    [Column("token_hash")]
    [MaxLength(128)]
    public string TokenHash { get; set; } = string.Empty;

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("revoked_at")]
    public DateTime? RevokedAt { get; set; }

    [Column("replaced_by_token_id")]
    public Guid? ReplacedByTokenId { get; set; }
}
