using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("pisos", Schema = "estructura")]
public class Piso
{
    [Key]
    public Guid Id { get; set; }

    [Column("edificio_id")]
    public Guid EdificioId { get; set; }

    [Column("numero")]
    public int Numero { get; set; }

    [Column("nombre")]
    [MaxLength(100)]
    public string? Nombre { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    public virtual Edificio Edificio { get; set; } = null!;
    public virtual ICollection<Ambiente> Ambientes { get; set; } = [];
}
