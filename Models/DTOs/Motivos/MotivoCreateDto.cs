using System.ComponentModel.DataAnnotations;

namespace HsqvLogistica.Models.DTOs.Motivos;

public class MotivoCreateDto
{
    [Required, StringLength(50)]
    public string Descripcion { get; set; } = null!;

    [Required, StringLength(10)]
    public string TipoMov { get; set; } = null!;
}
