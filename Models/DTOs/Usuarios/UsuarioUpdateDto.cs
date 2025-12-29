using System.ComponentModel.DataAnnotations;

namespace HsqvLogistica.Models.DTOs.Usuarios;

public class UsuarioUpdateDto
{
    [Required]
    public string Nombres { get; set; } = null!;

    [Required]
    public string Correo { get; set; } = null!;

    public string? Clave { get; set; } // opcional

    [Required]
    public int IdTipo { get; set; }
}
