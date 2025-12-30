namespace HsqvLogistica.Models.DTOs.Pedidos;

public class PedidoCreateDto
{
    public int IdCliente { get; set; }
    public int IdVendedor { get; set; }

    public DateOnly FechaEntrega { get; set; }

    public string Cliente { get; set; } = null!;
    public string? Detalles { get; set; }
    public string? Direccion { get; set; }

    public int? IdEmpServ { get; set; }

    public List<PedidoDetalleDto> DetallesPedido { get; set; } = new();
    public string UsuaCreacion { get; set; } = null!;
}
