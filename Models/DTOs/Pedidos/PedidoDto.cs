namespace HsqvLogistica.Models.DTOs.Pedidos;

public class PedidoDto
{
    public int Id { get; set; }

    public int? IdCliente { get; set; }
    public string? Cliente { get; set; }

    public int? IdVendedor { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public string? Detalles { get; set; }
    public string? Direccion { get; set; }

    public int? Estado { get; set; }
    public bool? Activo { get; set; }

    public List<PedidoDetalleDto> Detalle { get; set; } = new();
}
