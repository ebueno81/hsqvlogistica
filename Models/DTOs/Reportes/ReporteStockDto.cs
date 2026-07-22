namespace HsqvLogistica.Models.DTOs.Reportes;

public class ReporteStockDto
{
    public string Codigo { get; set; } = string.Empty;

    public string Articulo { get; set; } = string.Empty;

    public string Linea { get; set; } = string.Empty;

    public decimal Stock { get; set; }

    public string? Observacion { get; set; }
}