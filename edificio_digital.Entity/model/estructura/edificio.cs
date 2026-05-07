using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("edificios", Schema = "estructura")]
public class Edificio
{
    [Key]
    public Guid Id { get; set; }

    [Column("sede_id")]
    public Guid SedeId { get; set; }

    [Column("codigo")]
    [MaxLength(50)]
    public string Codigo { get; set; } = string.Empty;

    [Column("nombre")]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Column("activo")]
    public bool Activo { get; set; } = true;

    public virtual Sede Sede { get; set; } = null!;
    public virtual ICollection<Piso> Pisos { get; set; } = [];
}
