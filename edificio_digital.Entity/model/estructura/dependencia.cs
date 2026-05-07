using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edificio_digital.Entity.Model.Usuario;

namespace edificio_digital.Entity.Model;

[Table("dependencias", Schema = "estructura")]
public class Dependencia
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

    [Column("tipo")]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Column("activo")]
    public bool Activo { get; set; } = true;

    public virtual Sede Sede { get; set; } = null!;
    public virtual ICollection<Ambiente> Ambientes { get; set; } = [];
    public virtual ICollection<Equipo> Equipos { get; set; } = [];
    public virtual ICollection<edificio_digital.Entity.Model.Usuario.Usuario> Usuarios { get; set; } = [];
}
