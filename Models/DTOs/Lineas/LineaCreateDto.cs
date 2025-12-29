using System.ComponentModel.DataAnnotations;

namespace HsqvLogistica.Models.DTOs.Lineas
{
    public class LineaCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; } = null!;
    }
}
