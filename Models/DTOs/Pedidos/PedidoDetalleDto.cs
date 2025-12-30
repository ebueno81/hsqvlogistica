using HsqvLogistica.Models.DTOs.Articulos;

namespace HsqvLogistica.Models.DTOs.Pedidos;

public class PedidoDetalleDto
{
    public int? IdArticulo { get; set; }   // futuro lookup
    public string? Articulo { get; set; }  // texto libre por ahora
    public Decimal Cantidad { get; set; }
    public ArticuloLookupDto? ArticuloObj { get; set; }
}
