namespace HsqvLogistica.Models.DTOs.Reportes;

public class ReportePedidoDetalleDto
{
    public int IdPedido { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public string? Cliente { get; set; }

    public int? Estado { get; set; }

    public bool? Activo { get; set; }

    public string? CodigoArticulo { get; set; }

    public string? DescripcionArticulo { get; set; }

    public decimal Cantidad { get; set; }
}
