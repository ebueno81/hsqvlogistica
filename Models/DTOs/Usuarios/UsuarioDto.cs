namespace HsqvLogistica.Models.DTOs.Usuarios;

public class UsuarioDto
{
    public int Id { get; set; }
    public string? Usuario { get; set; }
    public string? Nombres { get; set; }
    public string? Correo { get; set; }

    public int? IdTipo { get; set; }
    public bool? Activo { get; set; }
    public string? TipoUsuario { get; set; }
}
