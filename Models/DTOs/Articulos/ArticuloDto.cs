namespace HsqvLogistica.Models.DTOs.Articulos;

public class ArticuloDto
{
    public int Id { get; set; }
    public string? Codigo { get; set; }
    public int? IdLinea { get; set; }
    public string? LineaDescripcion { get; set; }
    public string? Detalles { get; set; }
    public string? Descripcion { get; set; }
    public decimal? Stock { get; set; }
    public decimal? StockFijo { get; set; }
    public decimal? PrecioMn { get; set; }
    public decimal? PrecioUs { get; set; }
    public string? RutaImagen { get; set; }
    public bool? Activo { get; set; }
}
