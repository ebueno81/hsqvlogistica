namespace HsqvLogistica.Models.DTOs.Reportes;

public class ReporteKardexFilterDto
{
    public DateTime FechaDesde { get; set; }

    public DateTime FechaHasta { get; set; }

    public int? IdAlmacen { get; set; }

    public int? IdLinea { get; set; }

    public int? IdArticulo { get; set; }

    public string? TipoMovimiento { get; set; }
}