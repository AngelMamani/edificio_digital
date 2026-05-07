using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edificio_digital.Entity.Model;

[Table("sedes", Schema = "estructura")]
public class Sede
{
    [Key]
    public Guid Id { get; set; }

    [Column("codigo")]
    [MaxLength(50)]
    public string Codigo { get; set; } = string.Empty;

    [Column("nombre")]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Column("direccion")]
    [MaxLength(250)]
    public string? Direccion { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    public virtual ICollection<Edificio> Edificios { get; set; } = [];
    public virtual ICollection<Dependencia> Dependencias { get; set; } = [];
}
