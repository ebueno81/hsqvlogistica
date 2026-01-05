namespace HsqvLogistica.Models.DTOs.Garantia;

public class GarantiaDetalleDto
{
    public int IdArticulo { get; set; }
    public string? Articulo { get; set; }
    public decimal Cantidad { get; set; }
    public string? Detalles { get; set; }
}
