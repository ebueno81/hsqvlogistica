using HsqvLogistica.Models.DTOs.Pedidos;

public class PedidoPagedResultDto
{
    public List<PedidoDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
}
