using System.ComponentModel.DataAnnotations;

namespace HsqvLogistica.Models.DTOs.Articulos;

public class ArticuloUpdateDto
{
    [Required, StringLength(150)]
    public string Descripcion { get; set; } = null!;

    public int Stock { get; set; }
    public decimal PrecioMn { get; set; }
    public decimal PrecioUs { get; set; }

    public string RutaImagen { get; set; } = null!;
    public string Detalles { get; set; } = null!;
    public string UsuaModifica { get; set; } = null!;

    public int IdLinea { get; set; }
}
