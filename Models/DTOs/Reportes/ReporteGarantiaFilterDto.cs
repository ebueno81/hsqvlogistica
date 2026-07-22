namespace HsqvLogistica.Models.DTOs.Reportes;

public class ReporteGarantiaFilterDto
{
    public DateTime FechaDesde { get; set; }

    public DateTime FechaHasta { get; set; }

    public bool? Estado { get; set; }
}