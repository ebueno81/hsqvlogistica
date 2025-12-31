namespace HsqvLogistica.Models.DTOs.Pedidos;

public class PedidoFilterDto
{
    public string? Cliente { get; set; }
    public DateOnly? FechaDesde { get; set; }
    public DateOnly? FechaHasta { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
}
