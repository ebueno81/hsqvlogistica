using System.ComponentModel.DataAnnotations;

namespace HsqvLogistica.Models.DTOs.Articulos;

public class ArticuloCreateDto
{
    [Required, StringLength(20)]
    public string Codigo { get; set; } = null!;

    [Required]
    public int IdLinea { get; set; }

    [Required, StringLength(150)]
    public string Descripcion { get; set; } = null!;

    public int Stock { get; set; }
    public decimal PrecioMn { get; set; }
    public decimal PrecioUs { get; set; }
    public string Detalles { get; set; } = null!;

    public string RutaImagen { get; set; } = null!;
    public string UsuaCreacion { get; set; } = null!;


}
