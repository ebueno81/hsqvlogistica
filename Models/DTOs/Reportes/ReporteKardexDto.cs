namespace HsqvLogistica.Models.DTOs.Reportes;

public class ReporteKardexDto
{
    public int IdArticulo { get; set; }
    public string Codigo { get; set; } = "";
    public string Articulo { get; set; } = "";

    public decimal? StockInicial { get; set; }

    public DateTime Fecha { get; set; }

    public string TipoMov { get; set; } = "";
    public string Motivo { get; set; } = "";
    public string Guia { get; set; } = "";
    public string Cliente { get; set; } = "";

    public decimal? ING { get; set; }
    public decimal? SAL { get; set; }

    public decimal Saldo { get; set; }

    public string? Detalles { get; set; }
    public string? Linea { get; set; }
}