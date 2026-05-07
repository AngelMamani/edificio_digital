namespace edificio_digital.Entity.Auth;

public class UserCredentialEntity
{
    public Guid Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string? TipoUsuario { get; set; }
    public bool Activo { get; set; } = true;
    public string Contrasena { get; set; } = string.Empty;
}
