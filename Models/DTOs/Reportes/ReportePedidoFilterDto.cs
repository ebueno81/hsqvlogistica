
namespace HsqvLogistica.Models.DTOs.Reportes;

public class ReportePedidoFilterDto
{
    public DateOnly? FechaDesde { get; set; }
    public DateOnly? FechaHasta { get; set; }

    // Null = todos los estados
    public int? Estado { get; set; }

    // Null o vacío = todos los clientes
    public string? Cliente { get; set; }
}
