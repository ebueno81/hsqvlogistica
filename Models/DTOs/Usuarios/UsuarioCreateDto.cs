using System.ComponentModel.DataAnnotations;

namespace HsqvLogistica.Models.DTOs.Usuarios;

public class UsuarioCreateDto
{
    [Required]
    public string Usuario { get; set; } = null!;

    [Required]
    public string Nombres { get; set; } = null!;

    [Required]
    public string Correo { get; set; } = null!;

    [Required]
    public string Clave { get; set; } = null!;

    [Required]
    public int IdTipo { get; set; }
}
